using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperData
{
    public int isScrapped;
    public int uniqueId; 
    public int classId; 
    public string title;
    public string author;
    public int year;
    public string AI_T;
    public string AI_F; 


    public PaperData(int uniqueId, int classId, string title, string author, int year, string AI_T, string AI_F)
    {
        this.uniqueId = uniqueId;
        this.classId = classId;
        this.title = title;
        this.author =  author;
        this.year =  year;
        this.AI_T =  AI_T;
        this.AI_F = AI_F;
        this.isScrapped = 0;
    }
}
