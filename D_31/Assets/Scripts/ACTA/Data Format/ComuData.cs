using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComuData 
{
    public int isScrapped;
    public int uniqueId; // 0
    public int classId; 
    public string title;
    public string writer;
    public int like;
    public string content;

    public List<List<string>> comments;
    public int n_comments;

    public ComuData(int uniqueId, int classId, string title, string writer, int like, string content, List<List<string>> comments)
    {
        this.uniqueId = uniqueId;
        this.classId = classId;
        this.title = title;
        this.writer =  writer;
        this.like =  like;
        this.content =  content;
        this.isScrapped = 0;

        this.comments = comments; // 원소 리스트 : name, content 쌍 리스트 (대댓글 있으면 추가로 name, conten 하나 더 있음)
        this.n_comments = comments.Count;
    }
    
}
