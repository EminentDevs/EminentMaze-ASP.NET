using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//  Class representing the position of the elements in Element Class
public class Position
{
    public Position(byte row, byte col)
    {
        Col = col;
        Row = row;
    }

    public Position()
    {

    }

    public byte Col { set; get; }
    public byte Row { set; get; }
}