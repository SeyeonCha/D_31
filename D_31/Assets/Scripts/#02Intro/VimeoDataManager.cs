using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vimeo;
using Vimeo.SimpleJSON;
using System.Text.RegularExpressions;
using System;

// VideoLookupData 구조체 정의
[System.Serializable]
public class VideoLookupData
{
    public string FolderName; // 예: "Reels_D-31"
    public Dictionary<string, int> Videos = new Dictionary<string, int>(); // Key: Video Title, Value: Video ID
}

public class VimeoDataManager : MonoBehaviour
{
    // 싱글톤 패턴을 사용하여 어디서든 접근 가능하게 합니다.
    public static VimeoDataManager Instance { get; private set; }

    // 외부에서 토큰을 입력받기 위한 필드를 인스펙터에 노출합니다.
    [Header("Vimeo Settings")]
    [Tooltip("Vimeo API 토큰을 여기에 입력하세요.")]
    public string vimeoApiToken;

    // Vimeo API 컴포넌트
    private VimeoApi api;

    // 전체 Vimeo 데이터를 저장할 딕셔너리
    private Dictionary<string, VideoLookupData> allVimeoData = new Dictionary<string, VideoLookupData>();
    
    public bool IsDataLoaded { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeApiAndToken();
        
        if (api != null && !string.IsNullOrEmpty(api.token))
        {
            StartCoroutine(InitializeVimeoData());
        }
        else
        {
            Debug.LogError("[VimeoDataManager] Vimeo API Token이 설정되지 않았거나 VimeoApi 초기화에 실패했습니다. 데이터를 로드할 수 없습니다.");
        }
    }
    
    /// <summary>
    /// VimeoApi 컴포넌트를 확보하고 토큰을 설정합니다.
    /// </summary>
    private void InitializeApiAndToken()
    {
        // 1. 컴포넌트 확보 (없으면 추가)
        api = GetComponent<VimeoApi>();
        if (api == null)
        {
            api = gameObject.AddComponent<VimeoApi>();
            Debug.Log("[VimeoDataManager] VimeoApi 컴포넌트를 자동으로 추가했습니다.");
        }

        // 2. 토큰 주입
        // (VimeoApi의 token 필드가 public이므로 직접 주입 가능)
        if (!string.IsNullOrEmpty(vimeoApiToken))
        {
            api.token = vimeoApiToken;
        }
    }

    /// <summary>
    /// Vimeo API를 호출하여 모든 프로젝트(폴더)와 그 안의 영상 목록을 로드합니다.
    /// </summary>
    private IEnumerator InitializeVimeoData()
    {
        Debug.Log("[VimeoDataManager] Starting Vimeo data initialization...");
        
        // 1. 모든 프로젝트(폴더) 목록을 요청합니다.
        string projectSearchPath = "/me/projects?fields=name,uri";
        yield return SendApiRequest(projectSearchPath);

        if (!isRequestSuccessful)
        {
            Debug.LogError("[VimeoDataManager] Failed to load projects.");
            yield break;
        }

        JSONNode json;
        try
        {
            json = JSONNode.Parse(apiResponseResult);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[VimeoDataManager] Project JSON parsing error: {e.Message}");
            yield break;
        }

        if (json["data"] == null || json["data"].Count == 0)
        {
            Debug.LogWarning("[VimeoDataManager] No projects found in the Vimeo account.");
            IsDataLoaded = true;
            yield break;
        }

        // 2. 각 폴더(프로젝트)를 순회하며 내부 영상 목록을 요청합니다.
        foreach (JSONNode projectNode in json["data"].AsArray)
        {
            string folderName = projectNode["name"].Value;
            string projectUri = projectNode["uri"].Value;
            
            VideoLookupData folderData = new VideoLookupData {
                FolderName = folderName,
                Videos = new Dictionary<string, int>()
            };

            // 해당 폴더의 영상 목록 요청
            string videosSearchPath = $"{projectUri}/videos?fields=name,uri&per_page=100";
            yield return SendApiRequest(videosSearchPath);

            if (isRequestSuccessful)
            {
                try
                {
                    JSONNode videosJson = JSONNode.Parse(apiResponseResult);
                    if (videosJson["data"] != null)
                    {
                        // 3. 각 영상의 이름과 ID를 추출하여 저장합니다.
                        foreach (JSONNode videoNode in videosJson["data"].AsArray)
                        {
                            string videoName = videoNode["name"].Value;
                            string videoUri = videoNode["uri"].Value; // 예: /videos/123456789

                            Match match = Regex.Match(videoUri, "/([0-9]+)$");
                            if (match.Success && int.TryParse(match.Groups[1].Value, out int videoId))
                            {
                                folderData.Videos[videoName] = videoId;
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[VimeoDataManager] Videos JSON parsing error for {folderName}: {e.Message}");
                    continue;
                }
            }
            
            // 데이터 저장
            allVimeoData[folderName] = folderData;
            Debug.Log($"[VimeoDataManager] Loaded {folderData.Videos.Count} videos from folder: {folderName}");
        }

        IsDataLoaded = true;
        Debug.Log("[VimeoDataManager] All Vimeo data loaded successfully.");
    }
    
    // API 응답을 임시로 저장할 필드와 상태
    private string apiResponseResult;
    private bool isRequestComplete;
    private bool isRequestSuccessful;
    
    // API 요청 이벤트 핸들러
    private void ApiRequestComplete(string response)
    {
        apiResponseResult = response;
        isRequestComplete = true;
        isRequestSuccessful = true;
        api.OnRequestComplete -= ApiRequestComplete;
    }
    private void ApiError(string response)
    {
        apiResponseResult = response;
        isRequestComplete = true;
        isRequestSuccessful = false;
        api.OnError -= ApiError;
    }

    // API 요청 코루틴 래퍼
    private IEnumerator SendApiRequest(string apiPath)
    {
        isRequestComplete = false;
        isRequestSuccessful = false;
        
        // 요청마다 이벤트를 구독하고 처리 후 해제하여 코루틴이 충돌하지 않도록 합니다.
        api.OnRequestComplete += ApiRequestComplete;
        api.OnError += ApiError;
        
        yield return api.Request(apiPath);
        
        // 요청 완료 대기
        yield return new WaitUntil(() => isRequestComplete);
        
        if (!isRequestSuccessful)
        {
            Debug.LogError($"[VimeoDataManager] API Request Failed for {apiPath}: {apiResponseResult}");
        }
    }


    /// <summary>
    /// 저장된 데이터를 기반으로 영상 ID를 조회합니다.
    /// </summary>
    public int GetVideoId(string folderName, string videoTitle)
    {
        if (!IsDataLoaded)
        {
            Debug.LogError("[VimeoDataManager] Data is not yet loaded. Cannot retrieve ID.");
            return -1;
        }

        if (allVimeoData.TryGetValue(folderName, out VideoLookupData folderData))
        {
            if (folderData.Videos.TryGetValue(videoTitle, out int videoId))
            {
                return videoId;
            }
            Debug.LogWarning($"[VimeoDataManager] Video title '{videoTitle}' not found in folder '{folderName}'.");
            return -1;
        }
        
        Debug.LogWarning($"[VimeoDataManager] Folder '{folderName}' not found in loaded data.");
        return -1;
    }
}