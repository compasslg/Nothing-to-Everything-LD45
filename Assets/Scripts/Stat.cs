using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* ============================================================================================
 * A instance of this class represent a stat of an unit
 * Author: Hanwei Li (Compasslg)
 * ===========================================================================================*/
public class Stat {
	private string name;
	private float maxValue, bestValue, value, tempGain, addedPoint;
    /* ----------------------------------------------------------------------------------------
	 * The Constructors
	 * ----------------------------------------------------------------------------------------*/
     public Stat(string name, float value, bool hasBestValue)
    {
        this.name = name;
        this.value = value; // The current value
        maxValue = 9999; // The maximum potential value
        if (hasBestValue)
        {
            bestValue = value; // value <= bestValue
        }
        else
        {
            bestValue = 0; // No bestValue
        }
        tempGain = 0;
    }

    public Stat(string name, float value, float bestValue, float maxValue)
    {
        this.name = name;
        this.value = value;
        this.bestValue = bestValue;
        this.maxValue = maxValue;
        tempGain = 0;
    }
    /* ----------------------------------------------------------------------------------------
	 * Update The value of this stat
	 * Take a float which is to be added on the previous value
	 * ----------------------------------------------------------------------------------------*/
    public void UpdateValue(float gain){
		// Having a local variable will prevent bugs caused by the temporary value change
		float tempValue = value + gain;
        if(tempValue < 0) {
            value = 0;
        }
        else if (bestValue > 0 && tempValue > bestValue) {
			value = bestValue;
        }
        else
        {
            value = tempValue;
        }
	}
    /*----------------------------------------------------------------------------------------
     * 1. Add stat point, will be shown temporarily
     * 2. Confirm the point added, add it to the actual value and reset point to 0
     * 3. Reset points to zero, and return the value that was added.
     *----------------------------------------------------------------------------------------*/
     public void AddPoint()
    {
        addedPoint++;
    }
    public void ConfirmPoints()
    {
        if(bestValue != 0)
        {
            bestValue += addedPoint;
        }
        else
        {
            value += addedPoint;
        }
        addedPoint = 0;
    }
    public float ResetPoints()
    {
        float point = addedPoint;
        addedPoint = 0;
        return point;
    }
    /*----------------------------------------------------------------------------------------
	 * Update the value of this stat temporarily
	 * Take a float which is to be added on the previous value
	 *----------------------------------------------------------------------------------------*/
    public void TemporaryUpdate(float tempGain){
		value += tempGain;
		this.tempGain += tempGain;
	}

    /*----------------------------------------------------------------------------------------
	 * Resume all temporary update
	 * Limit the value with the range 0..bestValue (inclusive)
	 * ----------------------------------------------------------------------------------------*/
    public void ResumeTempUpdate(){
		// Having a local variable will prevent bugs caused by the temporary value change
		float tempValue = value - tempGain; 
		tempGain = 0;
		if (tempValue <= 0) {
			value = 1;
		} else if (tempValue > bestValue) {
			value = bestValue;
		} else {
			value = tempValue;
		}
	}

    /*----------------------------------------------------------------------------------------
	 * Getters
	 *----------------------------------------------------------------------------------------*/
    public float GetMaxValue(){
		return maxValue;
	}
	public float GetBestValue(){
		return bestValue;
	}
	public float GetValue(){
		return value;
	}
	public float GetTempGain(){
		return tempGain;
	}
	public string GetName(){
		return name;
	}
    public float GetEmptyValue()
    {
        return bestValue - value;
    }
    public float GetValuePercentage()
    {
        return value / bestValue;
    }
    // Override. Return the string representation of this stat
    public override string ToString()
    {
        if(bestValue > 0)
        {
            return (int)value + " / " + (int)(bestValue + addedPoint);
        }
        return (value + addedPoint).ToString();
    }
    public Stat GetNewCopy()
    {
        Stat copy = new Stat(name, value, bestValue, maxValue);
        return copy;
    }
    /*----------------------------------------------------------------------------------------
	 * Setters
	 *----------------------------------------------------------------------------------------*/
    public void SetMaxValue(float maxValue){
		this.maxValue = maxValue;
	}
	public void SetBestValue(float bestValue){
		this.bestValue = bestValue;
	}
	public void SetValue(float value){
		this.value = value;
	}
}

