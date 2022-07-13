using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable 
{
   public void Shoot();
   
} 

public interface IReloadable 
{

    public void Reload();

}


public interface IChargable
{


    public void Charge();
}