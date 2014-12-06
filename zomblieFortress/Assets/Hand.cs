using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

	public GameObject cardFab;

	public List<Card> cards = new List<Card>();
	public static int handSize = 5;

	public float leftCardPosition = -12f;
	public float cardSpacing = 6f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (cards.Count < handSize) {
			Vector3 pos = GetCardPosition(cards.Count + 1);
			GameObject card = (GameObject) Instantiate(cardFab, pos, Quaternion.identity);
			cards.Add(card.GetComponent<Card>());
		}
	}

	void ReposistionCards() {
		int count = 0;
		foreach (Card c in cards) {
			c.gameObject.transform.position = GetCardPosition(count);
			count++;
		}
	}

	Vector3 GetCardPosition(int count) {
		return new Vector3 (leftCardPosition + cardSpacing * count, gameObject.transform.position.y, -3);
	}
}
