
public class ButtonState
{
    public bool UpDisabled = false;
    public bool DownDisabled = false;
    public bool LeftDisabled = false;
    public bool RightDisabled = false;

    public void DisableAll()
    {
        UpDisabled = true;
        DownDisabled = true;
        LeftDisabled = true;
        RightDisabled = true;
    }

    public void EnableAll()
    {
        UpDisabled = false;
        DownDisabled = false;
        LeftDisabled = false;
        RightDisabled = false;
    }

    public void DisableGoBack(string direction)
    {
        EnableAll();
        switch (direction)
        {
            case "up":
                DownDisabled = true;
                break;
            case "down":
                UpDisabled = true;
                break;
            case "left":
                RightDisabled = true;
                break;
            case "right":
                LeftDisabled = true;
                break;
        }
    }
}