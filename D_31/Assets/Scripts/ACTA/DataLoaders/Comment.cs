using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment
{
    public string name;
    public string content;
    public string like;

    public Comment(string name, string content, string like)
    {
        this.name = name;
        this.content = content;
        this.like = like;
    }
}
