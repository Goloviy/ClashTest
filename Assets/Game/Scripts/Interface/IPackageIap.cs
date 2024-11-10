using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPackageIap 
{
    public string PackageId { get; }

    public Action<string> SelectPackage();
}
