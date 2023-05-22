using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ARWater : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button holdToDrinkButton;
    public Image waterCapacity;
    public TMP_Text timerText;

    int waterCounter;
    Color originalWaterColor;

    private Image buttonImage;
    bool timeToFillRefill = false;

    public bool timeToDrink = false;
    private bool isButtonDown = false;

    public TMP_Text waterCountText;

    private float fillSpeed = 0.35f;
    private float elapsedTime = 0f;
    private float timerElapsedTime = 0f; // Elapsed time for the countdown timer
    private float checkInterval = 15f;
    private bool isRefilling = false;
    private bool isConsequenceActive = false;

    private ARGameManager gameManager;
    private ARSpawner spawnerRef;

    private void Awake()
    {
        gameManager = FindObjectOfType<ARGameManager>();
        holdToDrinkButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        originalWaterColor = waterCapacity.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.groundPlacementRef.GroundSet())
        {
            // start the timer after ground is set
            spawnerRef = gameManager.groundPlacementRef.SpawnerRef;

            if (isButtonDown)
            {
                // button is held down

                // drinking
                if (waterCapacity.fillAmount > 0)
                {
                    waterCapacity.fillAmount -= fillSpeed * Time.deltaTime;
                }
                else
                {
                    // glass fully emptied 
                    if (!isRefilling)
                    {
                        waterCounter++;
                        waterCountText.text = waterCounter.ToString();

                        // reset drink timer
                        elapsedTime = 0f;
                    }

                    // automatically refill after glass is emptied
                    isRefilling = true;
                }
                
            }
            else if (!isRefilling && waterCapacity.fillAmount < 1)
            {
                // Automatically refill the glass when the button is released
                waterCapacity.fillAmount += fillSpeed * Time.deltaTime;
                if (waterCapacity.fillAmount >= 1)
                {
                    // glass full

                    isRefilling = false;
                }
            }

            if (isRefilling)
            {
                // refilling
                if (waterCapacity.fillAmount < 1)
                {
                    waterCapacity.fillAmount += fillSpeed * Time.deltaTime;
                }
                else
                {
                    isRefilling = false;
                }
            }

            // drink timer counter
            elapsedTime += Time.deltaTime;
            timerElapsedTime = (int)(checkInterval - elapsedTime);
            timerText.text = "Drink in " + timerElapsedTime + " s";

            if (elapsedTime >= checkInterval)
            {
                // time to drink
                if (!isButtonDown && !isConsequenceActive)
                {
                    // Start the consequence for not holding the button
                    StartConsequence();
                }

                // reset drink timer
                elapsedTime = 0f;
            }

            //if (isConsequenceActive)
            //{
            //    timerElapsedTime += Time.deltaTime;

            //    if (timerElapsedTime >= checkInterval)
            //    {
            //        timerElapsedTime = 0f;
            //        StopConsequence();
            //    }
            //    else
            //    {
            //        float timeLeft = checkInterval - timerElapsedTime;
            //        UpdateTimerText(timeLeft);
            //    }
            //}
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonDown = true;
        if (isConsequenceActive)
        {
            // Stop the consequence as the button is belatedly held down
            StopConsequence();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonDown = false;
    }

    private void StartConsequence()
    {
        isConsequenceActive = true;
        waterCapacity.color = Color.red;

        gameManager.groundPlacementRef.SpawnerRef.SpawnRandomFoeGamePiece();
    }

    private void StopConsequence()
    {
        isConsequenceActive = false;

        waterCapacity.color = originalWaterColor;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    public void ResetWaterCounter()
    {
        waterCounter = 0;
        waterCountText.text = waterCounter.ToString();
        waterCapacity.color = originalWaterColor;
    }

    private void UpdateTimerText(float time)
    {
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = "Drink in " + seconds + " s";
    }
}

