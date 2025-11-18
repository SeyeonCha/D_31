using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vimeo;
using Vimeo.SimpleJSON;
using System.Text.RegularExpressions;
using System;
using System.Linq; // Dictionary.ElementAt() 사용을 위해 추가

// VideoLookupData 구조체 정의
[System.Serializable]
public class VideoLookupData
{
    public string FolderName; // 예: "Reels_D-31"
    
    // Dictionary<Key: Video Title, Value: (Video ID, Thumbnail URL)>
    // 유니티 인스펙터 직렬화 제한으로 인해 Dictionary<string, T>는 직렬화되지 않으므로, 
    // 실제 데이터는 Dictionary를 사용하되, 인스펙터에서는 표시되지 않습니다.
    public Dictionary<string, (int videoId, string thumbnailUrl)> Videos = new Dictionary<string, (int videoId, string thumbnailUrl)>(); 
}

public class VimeoDataManager : MonoBehaviour
{
    // 싱글톤 패턴
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
            // 토큰이 없거나 API 초기화 실패 시 로드 건너뛰기
            Debug.LogError("[VimeoDataManager] Vimeo API Token이 설정되지 않았거나 VimeoApi 초기화에 실패했습니다. 데이터를 로드할 수 없습니다.");
            IsDataLoaded = true; // 로드가 실패했더라도 대기 상태를 해제합니다.
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
            // AddComponent를 사용하여 API 컴포넌트를 강제로 추가합니다.
            api = gameObject.AddComponent<VimeoApi>();
            Debug.Log("[VimeoDataManager] VimeoApi 컴포넌트를 자동으로 추가했습니다.");
        }

        // 2. 토큰 주입
        if (!string.IsNullOrEmpty(vimeoApiToken))
        {
            api.token = vimeoApiToken;
        }
        else if (!string.IsNullOrEmpty(api.token))
        {
            // 만약 API 컴포넌트에 토큰이 이미 설정되어 있다면 그 값을 vimeoApiToken에 저장합니다.
            vimeoApiToken = api.token;
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
            IsDataLoaded = true;
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
            IsDataLoaded = true;
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
                FolderName = folderName
            };

            // 썸네일 정보(pictures)를 요청 필드에 추가합니다.
            string videosSearchPath = $"{projectUri}/videos?fields=name,uri,pictures&per_page=100";
            yield return SendApiRequest(videosSearchPath);

            if (isRequestSuccessful)
            {
                try
                {
                    JSONNode videosJson = JSONNode.Parse(apiResponseResult);
                    if (videosJson["data"] != null)
                    {
                        // 3. 각 영상의 이름, ID, 썸네일 URL을 추출하여 저장합니다.
                        foreach (JSONNode videoNode in videosJson["data"].AsArray)
                        {
                            string videoName = videoNode["name"].Value;
                            string videoUri = videoNode["uri"].Value; 
                            string thumbnailUrl = null;

                            // 썸네일 URL 추출 로직: 가장 큰 썸네일 링크를 찾습니다.
                            if (videoNode["pictures"] != null && videoNode["pictures"]["sizes"].Count > 0)
                            {
                                // sizes 배열의 마지막 요소가 가장 큰 이미지 사이즈일 가능성이 높음
                                // 또는, 특정 해상도를 기준으로 선택할 수 있습니다. 여기서는 마지막(가장 큰) 것을 사용합니다.
                                JSONNode sizes = videoNode["pictures"]["sizes"];
                                // Vimeo API는 보통 큰 사이즈부터 정렬되어 있지만, 안전하게 마지막 요소를 가져옵니다.
                                thumbnailUrl = sizes[sizes.Count - 1]["link"].Value; 
                            }
                            
                            Match match = Regex.Match(videoUri, "/([0-9]+)$");
                            if (match.Success && int.TryParse(match.Groups[1].Value, out int videoId))
                            {
                                // (ID, Thumbnail URL) 튜플을 딕셔너리에 저장합니다.
                                folderData.Videos[videoName] = (videoId, thumbnailUrl);
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
    
    // API 요청 이벤트 핸들러 (SendApiRequest 코루틴이 기다리는 상태를 제어)
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
        
        api.OnRequestComplete += ApiRequestComplete;
        api.OnError += ApiError;
        
        yield return api.Request(apiPath);
        
        // 요청 완료 대기
        yield return new WaitUntil(() => isRequestComplete);
        
        // 에러 이벤트 핸들러 해제
        api.OnError -= ApiError; 
        
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
            if (folderData.Videos.TryGetValue(videoTitle, out var videoDetails))
            {
                return videoDetails.videoId;
            }
            Debug.LogWarning($"[VimeoDataManager] Video title '{videoTitle}' not found in folder '{folderName}'.");
            return -1;
        }
        
        Debug.LogWarning($"[VimeoDataManager] Folder '{folderName}' not found in loaded data.");
        return -1;
    }
    
    /// <summary>
    /// 저장된 데이터를 기반으로 썸네일 URL을 조회합니다.
    /// </summary>
    public string GetVideoThumbnailUrl(string folderName, string videoTitle)
    {
        if (!IsDataLoaded)
        {
            Debug.LogError("[VimeoDataManager] Data is not yet loaded. Cannot retrieve thumbnail URL.");
            return null;
        }

        if (allVimeoData.TryGetValue(folderName, out VideoLookupData folderData))
        {
            if (folderData.Videos.TryGetValue(videoTitle, out var videoDetails))
            {
                // 튜플의 두 번째 요소인 thumbnailUrl을 반환합니다.
                return videoDetails.thumbnailUrl;
            }
            Debug.LogWarning($"[VimeoDataManager] Video title '{videoTitle}' not found in folder '{folderName}'.");
            return null;
        }
        
        Debug.LogWarning($"[VimeoDataManager] Folder '{folderName}' not found in loaded data.");
        return null;
    }
}