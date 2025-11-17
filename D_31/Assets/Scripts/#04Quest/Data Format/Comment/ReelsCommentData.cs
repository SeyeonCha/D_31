using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelsCommentData 
{
    public string name;
    public string content;
    public string like;

    public ReelsCommentData(string name, string content, string like)
    {
        this.name = name;
        this.content = content;
        this.like = like;
    }
}
