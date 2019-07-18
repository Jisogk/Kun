using System;

public class DoorEventArgs : EventArgs
{
  public GameManager.Direction direction;
  public DoorEventArgs(GameManager.Direction dir)
  {
    direction = dir;
  }
}
