using System.Collections.Generic;
using DemoProject.Models;

public class DocumentAttributesModel
{
    public int docId {get;set;}
    public string name{get;set;}
    public AttributesModel[] models {get;set;}
}