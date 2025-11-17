using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentData
{
    public string name;
    public string content;
    public int n_reply;
    public CommentData reply; // 대댓글 저장 (한 개만)

    public CommentData(string name, string content, int n_reply = 0, string rname = null, string rcontent = null)
    {
        this.name = name;
        this.content = content;
        this.n_reply = n_reply;

        // 대댓글 저장
        if (this.n_reply >= 1 && !string.IsNullOrEmpty(rname))
        {
            // this.reply = new List<CommentData>()
            // {
            //     new CommentData(rname,rcontent)
            // };
            this.reply = new CommentData(rname, rcontent);
        }
        

    }
    public CommentData(string name, string content)
    {
        this.name = name;
        this.content = content;
        this.n_reply = 0; // 대댓글 자체는 또 다른 대댓글을 포함하지 않으므로 0
        // this.reply = new CommentData; // NullReferenceException 방지
    }
}
