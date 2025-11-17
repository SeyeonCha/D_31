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
    public int c_num; // 댓글 개수

    public List<CommentData> comments;

    public ComuData(int uniqueId, int classId, string title, string writer, int like, string content, int c_num, List<CommentData> comments)
    {
        this.uniqueId = uniqueId;
        this.classId = classId;
        this.title = title;
        this.writer =  writer;
        this.like =  like;
        this.content =  content;
        this.c_num = c_num;
        this.isScrapped = 0;

        this.comments = comments; // 원소 리스트 : name, content 쌍 리스트 (대댓글 있으면 추가로 name, conten 하나 더 있음)
    }
    
}
