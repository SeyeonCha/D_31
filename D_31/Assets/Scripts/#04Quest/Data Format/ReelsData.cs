using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelsData
{
    public int isScrapped;
    public int uniqueId; // 0
    public int classId; 
    public string title;
    public string youtuber;
    public string subs;
    public string views;
    public string like;

    public List<List<string>> comments;
    public int n_comments;


    public ReelsData(int uniqueId, int classId, string title, string youtuber, string subs, string views, string like, List<List<string>> comments)
    {
        this.uniqueId = uniqueId;
        this.classId = classId;
        this.title = title;
        this.youtuber =  youtuber;
        this.subs =  subs;
        this.views =  views;
        this.like =  like;
        this.isScrapped = 0;

        this.comments = comments; // 원소 리스트 : name, content 쌍 리스트
        this.n_comments = comments.Count;
    }
}
