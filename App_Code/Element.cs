using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//  Class representing each element in the maze
public class Element
{
    public Element()
    {
        
    }

    public Element(byte row, byte col)
    {
        position = new Position(row, col);//    Creates a new instance of the position object
    }

    public Position position;

    public byte Status;

    /*  EMPTY = 0
    *   BLOCK = 1
    *   MARKED = 2
    *   Solution = 3
    */
}