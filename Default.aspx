<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
    <link rel="icon" href="/favicon.ico" type="image/x-icon" />
    <script>var testArray = new Array; var testArray2 = new Array;</script>
    <link href="App_Themes/bootstrap-3.3.7-dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="App_Themes/StyleSheet.css" rel="stylesheet" />
    <script type="text/javascript" src="App_Themes/js/jquery-1.9.1.js"></script>
    <script src="App_Themes/bootstrap-3.3.7-dist/js/bootstrap.min.js"></script>
    <title>Maze</title>
</head>
<body>
    <form id="form1" runat="server" class="form-inline">

        <div class="onepage-wrapper">
            <section id="TitlePage">
                <div class="page_container">
                    <img src="App_Themes/images/logo.png" alt="EminentDevs" title="EminentDevs" width="40%" style="display: block; margin: 0 auto;">
                    <p>
                        <span style="text-align: center; display: block; font-size: 18pt;">Welcome to the EminentMaze!</span>
                        <br />
                        In this web application, you can see the visual solution to a generated maze.<br />
                        You have two options, to either create a Random Maze, or a PreMade Maze.<br />
                        Please notice that you may only create mazes with dimentions 4 × 4 to 50 × 50.<br />
                        In order to create your desired maze:
                    </p>

                    <ul style="list-style: decimal;">
                        <li>Select your desired maze type from the drop-down list.</li>
                        <li>Add your desired height and width.</li>
                        <li>Press the Create button.</li>
                    </ul>
                    <p>
                        In the new windows, you may press each of the buttons (except the start and finish point) to change the status of the button from block to free or vice versa, or press solve.<br />
                        <br />
                        If the maze is solvable, the solution length is appeared on the top of the window, and a button appears at the bottom of the windows to show you the step by step solution.<br />
                        If the maze is not solvable, you will see and error on the top of the window.
                    </p>
                    <div id="copyright">
                        Copyright© 2017.All Rights Reserved.
                        Powered By <a href="http://www.eminentdevs.com" target="_blank">EminentDevs</a>
                    </div>
                </div>
            </section>
            <section id="MazePage">
                <div class="page_container">
                    <asp:Label Text="Width " runat="server" /><asp:TextBox ID="Width" runat="server" TextMode="Number" CssClass="form-control" />
                    <asp:Label Text="Height " runat="server" /><asp:TextBox ID="Height" runat="server" TextMode="Number" CssClass="form-control" />
                    <br />
                    <br />
                    <asp:DropDownList ID="MazeSelectionList" runat="server" AutoPostBack="True" CssClass="form-control">
                        <asp:ListItem Selected="True">Premade Maze</asp:ListItem>
                        <asp:ListItem>Random Maze</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label Text="Percent (Default Value is 50%) " runat="server" ID="PercentLabel" Visible="false" /><asp:TextBox ID="Percent" runat="server" TextMode="Number" Visible="false" CssClass="form-control" />
                    <br />
                    <br />
                    <asp:Button ID="CreateRandomMaze_Button" Text="Create Random Maze" runat="server" OnClick="createRandomMaze_Click" Visible="false" CssClass="btn btn-primary" data-toggle="modal" data-target="#MazeBox" />
                    <asp:Button ID="CreatePremadeMaze_Button" Text="Create Premade Maze" runat="server" OnClick="createPremadeMaze_Click" Visible="true" CssClass="btn btn-primary" data-toggle="modal" data-target="#MazeBox" />
                    <br />
                    <!-- Modal -->
                    <div id="MazeBox" class="modal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" runat="server">
                        <div class="modal-dialog modal-lg modal-fc" role="document" runat="server">
                            <div class="modal-content modal-fc-mc" runat="server">
                                <div class="modal-header">
                                    <asp:Button Text="×" OnClick="CloseModal_Click" type="button" CssClass="close" data-dismiss="modal" aria-label="Close" runat="server" />
                                    <h4 class="modal-title" id="myModalLabel">Created Maze</h4>
                                </div>
                                <div class="modal-body">
                                    <asp:Label CssClass="solutionAnswer" ID="SolutionStatusLabel" runat="server" />
                                    <br />
                                    <asp:Label CssClass="SelectedBox" ID="ClickedBoxIDLabel" runat="server" />

                                    <div id="MazeChart" runat="server" visible="true">
                                    </div>
                                    <br />
                                    <br />
                                    <asp:Button ID="SolveRandomMaze_Button" runat="server" OnClick="SolveRandom_Click" Text="Solve Random Maze" Visible="false" CssClass="btn btn-primary" />
                                    <asp:Button ID="SolvePremadeMaze_Button" runat="server" OnClick="SolvePreMade_Click" Text="Solve Premade Maze" Visible="true" CssClass="btn btn-primary" />
                                    <asp:Button ID="ShowSolution_Button" OnClientClick="ShowSolution1(); return false;" Text="Show Solution" Visible="false" runat="server" CssClass="btn btn-success" />
                                    <br />
                                </div>

                            </div>
                        </div>
                    </div>
                    <!-- End of Modal -->
                </div>
            </section>
        </div>

    </form>
    <script type="text/javascript">
        var i = testArray.length - 1;
        var j = testArray2.length - 1;
        //ShowSolution1 and ShowSolution2 functions will show the solution of the maze together by combining delay functions
        //The data required for these functions are provided using the RegisterStartupScript method in codebehind.
        function ShowSolution1() {
            setTimeout(function () {
                document.getElementById(testArray[i + 1]).style.backgroundColor = 'LightGrey';
                document.getElementById(testArray[i + 1]).style.backgroundSize = "100% auto";
                document.getElementById(testArray[i + 1]).style.backgroundImage = "none";
            }, 150)

            setTimeout(function () {
                //document.getElementById(testArray[i]).style.backgroundColor = 'syan';
                setTimeout(function () {
                    document.getElementById(testArray[i]).style.backgroundImage = "url('App_Themes/images/mouse-icon.png')";
                }, 150)

                i--;
                if (i >= 0) {
                    ShowSolution1();
                }
                else {
                    document.getElementById(testArray[i + 1]).style.backgroundColor = 'red';
                    ShowSolution2();
                }
            }, 200)
        }
        function ShowSolution2() {
            setTimeout(function () {
                document.getElementById(testArray2[j]).style.backgroundColor = 'gold';
                j--;
                if (j >= 0) {
                    ShowSolution2();
                }
                else {
                    document.getElementById(testArray2[j + 1]).style.backgroundColor = 'red'
                    return false;
                }
            }, 0)
        }
    </script>
    <%--    <audio src="App_Themes/Audio/The%20SLS%20AMG%20Electric%20Drive.mp3" controls="controls" autoplay="autoplay" loop="loop" />--%>
</body>
</html>
