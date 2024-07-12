using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSelect : MonoBehaviour
{
    public abstract void Select(bool isSelect);
}
public interface ISelect
{
    public abstract void Select();
    public abstract void UnSelect();
}