using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgductIapPackage
{
    void Init();
    string ProductId { get; }

    void PurchaseSuccess();
    void PurchaseFail();
}
