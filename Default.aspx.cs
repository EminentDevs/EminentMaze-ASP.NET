using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Text;
using System.Runtime;

public partial class _Default : System.Web.UI.Page
{
    //  This function loads when the ASP.NET page is loaded
    protected void Page_Load(object sender, EventArgs e)
    {
        //Condition runs if "PreMade Maze" is selected in drop-down list 
        if (MazeSelectionList.SelectedValue == "Premade Maze")
        {
            CreatePremadeMaze_Button.Visible = true;
            SolvePremadeMaze_Button.Visible = true;
            CreateRandomMaze_Button.Visible = false;
            SolveRandomMaze_Button.Visible = false;
            PercentLabel.Visible = false;
            Percent.Visible = false;
        }
        //Condition runs if "Random Maze" is selected in drop-down list 
        if (MazeSelectionList.SelectedValue == "Random Maze")
        {
            CreatePremadeMaze_Button.Visible = false;
            SolvePremadeMaze_Button.Visible = false;
            CreateRandomMaze_Button.Visible = true;
            SolveRandomMaze_Button.Visible = true;
            PercentLabel.Visible = true;
            Percent.Visible = true;
        }
        //Condition runs if page is postback
        if (IsPostBack)
        {
            DisplayMaze();
            Control PostBack = new Control();
            PostBack = GetPostBackControl(Page);
            bool MazeBoxPostBack = false;
            for (int i = 0; i <= 50; i++)
            {
                for (int j = 0; j <= 50; j++)
                {
                    string i_id = (i >= 10) ? i.ToString() : string.Format("0{0}", i);
                    string j_id = (j >= 10) ? j.ToString() : string.Format("0{0}", j);
                    if (PostBack.ID == string.Format("B_{0}_{1}", i_id, j_id))
                    {
                        MazeBoxPostBack = true;
                        break;
                    }
                }
                if (MazeBoxPostBack == true)
                {
                    break;
                }
            }
            //  This condition runs if any of the specific buttons are clicked:
            /*  CreateRandomMaze_Button
             *  CreatePremadeMaze_Button
             *  SolveRandomMaze_Button
             *  SolvePremadeMaze_Button
             *  ShowSolution_Button
             */
            if (MazeBoxPostBack == true || PostBack.ID == "CreateRandomMaze_Button" || PostBack.ID == "CreatePremadeMaze_Button"
                || PostBack.ID == "SolveRandomMaze_Button" || PostBack.ID == "SolvePremadeMaze_Button" || PostBack.ID == "ShowSolution_Button")
            {
                MazeBox.Attributes.CssStyle.Add("display", "block");
                MazeBox.Attributes.CssStyle.Add("overflow-y", "auto");
            }
            //For other postbacks, the Modal becomes invisible
            else
                MazeBox.Attributes.CssStyle.Add("display", "none");
        }
        //If the page is not a postback (it is refreshed or opened for the first time), the Maze session is removed.
        else
        {
            Session.Remove("Maze");
        }
    }

    //  This function gets the ID of the button that the page is posted back from.
    public static Control GetPostBackControl(Page page)
    {
        Control control = null;

        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }

    //  This function is used to display the maze in the Modal
    private void DisplayMaze()
    {
        Maze maze = (Maze)Session["Maze"];//    The session is take and put into and object pointer
        MazeChart.Controls.Clear();//   The maze controls are now cleared
        if (maze != null)
        {
            for (int i = 0; i < maze.Height; i++)
            {
                for (int j = 0; j < maze.Width; j++)
                {
                    Button temp = new Button();
                    temp.Click += element_Click;
                    string i_id = (i >= 10) ? i.ToString() : string.Format("0{0}", i);
                    string j_id = (j >= 10) ? j.ToString() : string.Format("0{0}", j);
                    temp.ID = string.Format("B_{0}_{1}", i_id, j_id);// Buttons are given IDs to be recognizable for displaying
                    if (maze.Map[i, j].Status == 0)
                    {
                        temp.BackColor = Color.LightGray;// Sets the color of empty boxes
                    }
                    else
                    {
                        temp.BackColor = Color.Black;// Sets the color of block boxes
                    }
                    //Start Point
                    if (i == 0 && j == 0)
                        temp.BackColor = Color.Green;// Sets the color of starting point
                    //Finish Point
                    if (i == maze.Height - 1 && j == maze.Width - 1)
                        temp.BackColor = Color.Red;//   Sets the color of finish point
                    MazeChart.Controls.Add(temp);
                }
                MazeChart.Controls.Add(new Label { Text = "<br/>" });// Goes to the next line to show the next row of the maze
            }
        }

    }

    //  This function shows the solution of the maze
    private void DisplayMazeSolution()
    {
        Maze maze = (Maze)Session["Maze"];
        DisplayMaze();

        //  Creates an array showing all of the possible ways before actually solving the maze
        Element[] allWayArray = new Element[maze.allWayToSolution.Count];
        allWayArray = maze.allWayToSolution.ToArray();
        List<string> tempString = new List<string>();

        foreach (Element item in allWayArray)
        {
            string i_id = (item.position.Row >= 10) ? item.position.Row.ToString() : string.Format("0{0}", item.position.Row);
            string j_id = (item.position.Col >= 10) ? item.position.Col.ToString() : string.Format("0{0}", item.position.Col);
            tempString.Add(string.Format("B_{0}_{1}", i_id, j_id));
        }

        //  Creating a string representing a javascript that sends the ID of the marked boxes to the client
        StringBuilder sb = new StringBuilder();
        sb.Append("<script>");
        sb.Append("var testArray = new Array;");
        foreach (string str in tempString)
        {
            if (str != "B_00_00")
            {
                sb.Append("testArray.push('" + str + "');");
            }
        }
        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "TestArrayScript", sb.ToString());// The function used for sending the script
        ShowSolution_Button.Visible = true;

        List<string> tempString2 = new List<string>();

        foreach (Element item in maze.solution)
        {
            string i_id = (item.position.Row >= 10) ? item.position.Row.ToString() : string.Format("0{0}", item.position.Row);
            string j_id = (item.position.Col >= 10) ? item.position.Col.ToString() : string.Format("0{0}", item.position.Col);
            tempString2.Add(string.Format("B_{0}_{1}", i_id, j_id));
        }

        StringBuilder sb2 = new StringBuilder();
        sb2.Append("<script>");
        sb2.Append("var testArray2 = new Array;");
        foreach (string str in tempString2)
        {
            if (str != "B_00_00")
            {
                sb2.Append("testArray2.push('" + str + "');");
            }
        }
        sb2.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "TestArrayScript2", sb2.ToString());

        ((Button)MazeChart.Controls[0]).BackColor = Color.Green;
        ((Button)MazeChart.Controls[elementIndexInMazeChart(maze.Map[maze.Height - 1, maze.Width - 1].position.Row, maze.Map[maze.Height - 1, maze.Width - 1].position.Col)]).BackColor = Color.Red;
    }

    //  This function runs when any of the maze boxes are clicked
    private void element_Click(object sender, EventArgs e)
    {
        Maze maze = (Maze)Session["Maze"];
        string ID = (sender as Button).ID;//    Gets the ID of the clicked maze box
        string FinishRow, FinishCol;

        if (maze.Height < 10)
            FinishRow = string.Format("0{0}", (maze.Height - 1));
        else
            FinishRow = (maze.Height - 1).ToString();

        if (maze.Width < 10)
            FinishCol = string.Format("0{0}", (maze.Width - 1));
        else
            FinishCol = (maze.Width - 1).ToString();
        /*  If the selected box is the start of finish point, nothing happens
         *  If the selected Box is a block, it turns to an empty box and vice versa
         */
        if (ID == "B_00_00" || ID == string.Format("B_{0}_{1}", FinishRow, FinishCol))
            return;
        if (maze.Map[byte.Parse(ID.Substring(2, 2)), byte.Parse(ID.Substring(5, 2))].Status == 0)
            maze.Map[byte.Parse(ID.Substring(2, 2)), byte.Parse(ID.Substring(5, 2))].Status = 1;
        else
            maze.Map[byte.Parse(ID.Substring(2, 2)), byte.Parse(ID.Substring(5, 2))].Status = 0;
        string SelectedID = (sender as Button).ID;
        ClickedBoxIDLabel.Text = string.Format("Last Selected Box >> Height : {0} , Width : {1}", Convert.ToInt16(SelectedID.Substring(2, 2)) + 1, Convert.ToInt16(SelectedID.Substring(5, 2)) + 1);//  Shows the latest clicked maze box
        SolutionStatusLabel.Text = "";
        DisplayMaze();
    }

    //  This function runs when Random Maze button is clicked
    protected void createRandomMaze_Click(object sender, EventArgs e)
    {
        Session.Remove("Maze");//   If a maze already exists, it is removed
        MazeChart.Controls.Clear();

        Maze maze;
        byte width;
        byte height;
        //  Adding dimensions to the maze
        //  It is limited from 4 * 4 to 50 * 50 
        if (string.IsNullOrEmpty(Width.Text) || Convert.ToInt16(Width.Text) < 4)
            width = (byte)4;
        else if (Convert.ToInt16(Width.Text) > 50)
            width = (byte)50;
        else
            width = Convert.ToByte(Width.Text);
        //----------
        if (string.IsNullOrEmpty(Height.Text) || Convert.ToInt16(Height.Text) < 4)
            height = (byte)4;
        else if (Convert.ToInt16(Height.Text) > 50)
            height = (byte)50;
        else
            height = Convert.ToByte(Height.Text);
        //----------
        maze = new Maze(height, width);

        maze.Start_Position = new Element(0, 0);
        maze.Finish_Position = new Element((byte)(maze.Height - 1), (byte)(maze.Width - 1));

        int NumberOfBlocks;

        if (string.IsNullOrEmpty(Percent.Text))
            NumberOfBlocks = ((width * height) * 50) / 100;
        else
            NumberOfBlocks = ((width * height) * Convert.ToInt32(Percent.Text)) / 100;
        Random random = new Random();
        bool RandomOK = true;
        //  Creates random blocks
        for (int i = 0; i < NumberOfBlocks; i++)
        {
            while (RandomOK)
            {
                int a = random.Next(0, maze.Height);
                int b = random.Next(0, maze.Width);
                if (maze.Map[a, b].Status == 0 &&
                    !(maze.Start_Position.position.Row == a && maze.Start_Position.position.Col == b) &&
                    !(maze.Finish_Position.position.Row == a && maze.Finish_Position.position.Col == b)
                )
                {
                    maze.Map[a, b].Status = 1;
                    break;
                }
            }
        }
        Session["Maze"] = maze;//   Creates a new session containing the created maze

        DisplayMaze();
    }

    //   This function runs when Solving Random Maze button is clicked
    protected void SolveRandom_Click(object sender, EventArgs e)
    {
        Maze maze = (Maze)Session["Maze"];
        if (maze != null)
        {
            maze.Solve();// Calls the Solve function from the Maze Class
            if (maze.solution != null)//    Runs if the maze was solvable
            {
                SolutionStatusLabel.Text = "The Maze is Solvable!" + "<br />" + "Solution Length: " + maze.solution.Length;
                DisplayMazeSolution();
            }
            else//  Run if the maze does not have an answer
            {
                SolutionStatusLabel.Text = "This Maze Is Not Solvable!";
            }
        }
    }

    //  This function is used for getting the index of the boxes in the chart
    private int elementIndexInMazeChart(byte i, byte j)
    {
        Maze maze = (Maze)Session["Maze"];
        return i * (maze.Width + 1) + j;
    }

    //  This function runs when Creating PreMade Maze button is clicked
    protected void createPremadeMaze_Click(object sender, EventArgs e)
    {
        Session.Remove("Maze");
        MazeChart.Controls.Clear();

        Stack<Element> pMMPath = new Stack<Element>();//    Stack used for creating a premade maze using depth-first method, using the reverse method for creating the maze

        Random rand = new Random();

        byte EspNum;

        byte width;
        byte height;
        //----------
        if (string.IsNullOrEmpty(Width.Text) || Convert.ToInt16(Width.Text) < 5)
            width = (byte)5;
        else if (Convert.ToInt16(Width.Text) > 50)
            width = (byte)50;
        else
            width = Convert.ToByte(Width.Text);
        //----------
        if (string.IsNullOrEmpty(Height.Text) || Convert.ToInt16(Height.Text) < 5)
            height = (byte)5;
        else if (Convert.ToInt16(Height.Text) > 50)
            height = (byte)50;
        else
            height = Convert.ToByte(Height.Text);

        Maze pMaze = new Maze(height, width);

        for (int i = 0; i < pMaze.Height; i++)
        {
            for (int j = 0; j < pMaze.Width; j++)
            {
                pMaze.Map[i, j].Status = 1;
            }
        }

        Session["Maze"] = pMaze;

        pMMPath.Push(pMaze.Map[0, 0]);

        byte random;

        //  Randomly chooses one of the available boxed from top, bottom, laft, and right
        while (pMMPath.Count != 0)
        {
            random = (byte)rand.Next(0, (int)numberOfAvaialbleNeighbors(pMMPath.Peek()));
            EspNum = 0;

            if (Check_Second_rightNeighborUnmarked(pMMPath.Peek()))
            {
                if (random == EspNum)
                {
                    pMaze.Map[pMMPath.Peek().position.Row, pMMPath.Peek().position.Col + 1].Status = 0;
                    pMaze.Map[pMMPath.Peek().position.Row, pMMPath.Peek().position.Col + 2].Status = 0;
                    pMMPath.Push(pMaze.Map[pMMPath.Peek().position.Row, pMMPath.Peek().position.Col + 2]);
                    continue;
                }
                else
                {
                    EspNum++;
                }
            }

            if (Check_Second_buttomNeighborUnmarked(pMMPath.Peek()))
            {
                if (random == EspNum)
                {
                    pMaze.Map[pMMPath.Peek().position.Row + 1, pMMPath.Peek().position.Col].Status = 0;
                    pMaze.Map[pMMPath.Peek().position.Row + 2, pMMPath.Peek().position.Col].Status = 0;
                    pMMPath.Push(pMaze.Map[pMMPath.Peek().position.Row + 2, pMMPath.Peek().position.Col]);
                    continue;
                }
                else
                {
                    EspNum++;
                }
            }

            if (Check_Second_leftNeighborUnmarked(pMMPath.Peek()))
            {
                if (random == EspNum)
                {
                    pMaze.Map[pMMPath.Peek().position.Row, pMMPath.Peek().position.Col - 1].Status = 0;
                    pMaze.Map[pMMPath.Peek().position.Row, pMMPath.Peek().position.Col - 2].Status = 0;
                    pMMPath.Push(pMaze.Map[pMMPath.Peek().position.Row, pMMPath.Peek().position.Col - 2]);
                    continue;
                }
                else
                {
                    EspNum++;
                }
            }

            if (Check_Second_topNeighborUnmarked(pMMPath.Peek()))
            {
                if (random == EspNum)
                {
                    pMaze.Map[pMMPath.Peek().position.Row - 1, pMMPath.Peek().position.Col].Status = 0;
                    pMaze.Map[pMMPath.Peek().position.Row - 2, pMMPath.Peek().position.Col].Status = 0;
                    pMMPath.Push(pMaze.Map[pMMPath.Peek().position.Row - 2, pMMPath.Peek().position.Col]);
                    continue;
                }
                else
                {
                    EspNum++;
                }
            }

            pMMPath.Pop();
        }
        pMaze.Start_Position = new Element(0, 0);
        pMaze.Finish_Position = new Element((byte)(pMaze.Height - 1), (byte)(pMaze.Width - 1));
        pMaze.Map[(byte)(pMaze.Height - 1), (byte)(pMaze.Width - 1)].Status = 0;
        Session["Maze"] = pMaze;
        DisplayMaze();
    }

    #region Solve Premade Maze
    //  This function runs when Solving PreMade Maze button is clicked
    protected void SolvePreMade_Click(object sender, EventArgs e)
    {
        Maze maze = (Maze)Session["Maze"];
        if (maze != null)
        {
            maze.Solve();
            if (maze.solution != null)
            {
                SolutionStatusLabel.Text = "The Maze is Solvable!" + "<br />" + "Solution Length: " + maze.solution.Length;
                DisplayMazeSolution();
            }
            else
            {
                SolutionStatusLabel.Text = "This Maze Is Not Solvable!";
            }
        }
    }
    #endregion

    private byte numberOfAvaialbleNeighbors(Element element)
    {
        byte count = 0;
        bool top, right, bottom, left;
        top = Check_Second_topNeighborUnmarked(element);
        right = Check_Second_rightNeighborUnmarked(element);
        bottom = Check_Second_buttomNeighborUnmarked(element);
        left = Check_Second_leftNeighborUnmarked(element);

        if (top == true)
            count++;
        if (right == true)
            count++;
        if (bottom == true)
            count++;
        if (left == true)
            count++;

        return count;
    }

    #region Check Second Neighbors Unmarked
    //  This functions checks if the second right neighbor of the current box is available
    bool Check_Second_rightNeighborUnmarked(Element input)
    {
        Maze pMaze = (Maze)Session["Maze"];
        if (input.position.Col + 2 < pMaze.Width)
        {
            if (pMaze.Map[input.position.Row, input.position.Col + 2].Status == 1)
                return true;
        }
        return false;
    }

    //  This functions checks if the second bottom neighbor of the current box is available
    bool Check_Second_buttomNeighborUnmarked(Element input)
    {
        Maze pMaze = (Maze)Session["Maze"];
        if (input.position.Row + 2 < pMaze.Height)
        {
            if (pMaze.Map[input.position.Row + 2, input.position.Col].Status == 1)
                return true;
        }
        return false;
    }

    //  This functions checks if the second left neighbor of the current box is available
    bool Check_Second_leftNeighborUnmarked(Element input)
    {
        Maze pMaze = (Maze)Session["Maze"];
        if (input.position.Col - 2 >= 0)
        {
            if (pMaze.Map[input.position.Row, input.position.Col - 2].Status == 1)
                return true;
        }
        return false;
    }

    //  This functions checks if the second top neighbor of the current box is available
    bool Check_Second_topNeighborUnmarked(Element input)
    {
        Maze pMaze = (Maze)Session["Maze"];
        if (input.position.Row - 2 >= 0)
        {
            if (pMaze.Map[input.position.Row - 2, input.position.Col].Status == 1)
                return true;
        }
        return false;
    }
    #endregion

    //  This function redirects the client to the home page when Modal's close button is clicked
    protected void CloseModal_Click(object sender, EventArgs e)
    {
        Response.Redirect("/");
    }
}
 
 
 
 
 