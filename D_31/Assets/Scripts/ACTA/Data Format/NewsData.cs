using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsData
{
    public int isScrapped;
    public int uniqueId; // 0
    public int classId; 
    public string title;
    public string reporter;
    public int like;
    public int dislike;
    public string content; // 6

    public List<List<string>> comments;
    public int n_comments;


    public NewsData(int uniqueId, int classId, string title, string reporter, int like, int dislike, string content, List<List<string>> comments)
    {
        this.uniqueId = uniqueId;
        this.classId = classId;
        this.title = title;
        this.reporter =  reporter;
        this.like =  like;
        this.dislike =  dislike;
        this.content =  content;
        this.isScrapped = 0;

        this.comments = comments; // 원소 리스트 : name, content 쌍 리스트
        this.n_comments = comments.Count;
    }
}
