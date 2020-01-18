using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class IAIState : MonoBehaviour {

    // Use this for initialization
    public ICharactor m_charactor;
    public List<ICharactor> m_targets;
    public string m_stateName = "";
    public void SetCharactor(ICharactor charactor)
    {
        m_charactor = charactor;
    }
    public virtual void  InitState(ICharactor charactor,List<ICharactor> targets = null,string stateName = "")
    {
        m_charactor = charactor;
        m_targets = targets;
        m_stateName = stateName;
    }

    public virtual void UpdateState()
    {

    }
}
