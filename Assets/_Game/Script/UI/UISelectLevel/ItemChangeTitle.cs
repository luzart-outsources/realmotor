

public class ItemChangeTitle : ButtonSelect
{
    protected override void Start()
    {
        base.Start();
        GameUtil.ButtonOnClick(btn, ClickAction, IsAnim);
    }
}
