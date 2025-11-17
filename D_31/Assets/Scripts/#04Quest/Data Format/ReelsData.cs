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

    public List<ReelsCommentData> comments; // name, content, like으로 이루어진 구조체
    public int c_num;


    public ReelsData(int uniqueId, int classId, string title, string youtuber, string subs, string views, string like, int c_num, List<ReelsCommentData> comments)
    {
        this.uniqueId = uniqueId;
        this.classId = classId;
        this.title = title;
        this.youtuber =  youtuber;
        this.subs =  subs;
        this.views =  views;
        this.like =  like;
        this.isScrapped = 0;

        this.comments = comments; // 리스트 원소 : // name, content, like으로 이루어진 구조체
        this.c_num = c_num;
    }
}
