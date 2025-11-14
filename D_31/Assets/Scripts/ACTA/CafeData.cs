using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeData
{
    public int isScrapped;
    public int uniqueId; // 0
    public int classId; 
    public string title;
    public string writer;
    public int views; // 4
    public int like; // 5
    public string content; // 6

    public List<List<string>> comments;
    public int n_comments;


    public CafeData(int uniqueId, int classId, string title, string writer, int views, int like, string content, List<List<string>> comments)
    {
        this.uniqueId = uniqueId;
        this.classId = classId;
        this.title = title;
        this.writer =  writer;
        this.views =  views;
        this.like =  like;
        this.content =  content;
        this.isScrapped = 0;

        this.comments = comments; // 원소 리스트 : name, content 쌍 리스트
        this.n_comments = comments.Count; // 댓글 개수
        
    }
}
