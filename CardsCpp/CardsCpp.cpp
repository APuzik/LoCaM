// Cards.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <iostream>
#include <string>
#include <vector>
#include <algorithm>
#include <sstream>
#include <map>

using namespace std;

/**
* Auto-generated code below aims at helping you parse
* the standard input according to the problem statement.
**/
struct Player
{
	int health;
	int maxMana;
	int sizeDeck;
	int rune;
	int sizeHand;
};

struct Card
{
	int number;
	int id;
	int location;
	int type;
	int cost;
	int attack;
	int defense;
	string abils;
	int myHealthChange;
	int oppHealthChange;
	int draw;

	bool isCharge()const
	{
		return abils.find('C') != string::npos;
	}

	bool isGuard()const
	{
		return abils.find('G') != string::npos;
	}

	bool isTrample()const
	{
		return abils.find('B') != string::npos;
	}
};

vector<Player> players(2);
vector<Card> myHand;
vector<Card> myCards;
vector<Card> oppCards;


bool isDraft(int turn)
{
	return turn < 30;
}

void printCommands(const vector<string>& commands)
{
	if (commands.empty())
	{
		cout << "PASS" << endl;
		return;
	}

	cout << commands[0];
	for (int i = 1; i < commands.size(); i++)
		cout << ";" << commands[i];

	cout << endl;
	cout.flush();
}

string draftCards(const vector<Card>& draft)
{
	int selected = 0;

	for (int i = 1; i < draft.size(); i++)
	{
		if (draft[i].attack == 0)
		{
			if (draft[selected].attack == 0)
			{
				if (draft[i].cost < draft[selected].cost)
				{
					selected = i;
				}
			}
		}
		else
		{
			if (draft[selected].attack == 0)
			{
				selected = i;
			}
			else
			{
				bool isChargeI = draft[i].isCharge();
				bool isChargeS = draft[selected].isCharge();
				if (isChargeI == isChargeS)
				{
					if (draft[i].cost == draft[selected].cost)
					{
						if (draft[i].attack == draft[selected].attack)
						{
							if (draft[i].defense > draft[selected].defense)
							{
								selected = i;
							}
						}
						else if (draft[i].attack > draft[selected].attack)
						{
							selected = i;
						}
					}
					else if (draft[i].cost < draft[selected].cost)
					{
						selected = i;
					}
				}
				else if (isChargeI)
				{
					selected = i;
				}
			}
		}

	};

	stringstream ss;
	ss << "PICK " << selected;

	return ss.str();
}

void sortMyHand()
{
	sort(myHand.begin(), myHand.end(), [](Card& c1, Card& c2)
	{
		if (c1.cost == c2.cost)
		{
			bool isChargeI = c1.isCharge();
			bool isChargeS = c2.isCharge();
			if (isChargeI == isChargeS)
			{
				if (c1.attack == c2.attack)
				{
					return c1.defense < c2.defense;
				}

				return c1.attack < c2.attack;
			}
			else
			{
				return isChargeI;
			}
		}

		return c1.cost < c2.cost;
	});
}

string getSummonCommand(const Card& card)
{
	stringstream ss;
	ss << "SUMMON " << card.id;

	return ss.str();
}

void getSummonCommands(vector<string>& commands)
{
	int curMana = players[0].maxMana;

	for (int i = 0; i < myHand.size() && curMana > 0; i++)
	{
		if (myHand[i].cost > curMana)
			break;

		curMana -= myHand[i].cost;
		commands.emplace_back(getSummonCommand(myHand[i]));
		if (myHand[i].isCharge())
			myCards.push_back(myHand[i]);
	}
}

string getAttackCommand(const Card& card, int id)
{
	stringstream ss;
	ss << "ATTACK " << card.id << " " << id;

	return ss.str();
}

int getBestAttacker(int guard)
{
	int id = -1;
	int bestAttack = 10000;
	for (int i = 0; i < myCards.size(); i++)
	{
		if (myCards[i].attack >= oppCards[guard].defense && myCards[i].attack < bestAttack)
		{
			bestAttack = myCards[i].attack;
			id = i;
		}
	}

	return id;
}

void getAttackCommands(vector<string>& commands)
{
	vector<int> guards;
	for (int i = 0; i < oppCards.size(); i++)
	{
		if (oppCards[i].isGuard())
			guards.push_back(i);
	}

	sort(guards.begin(), guards.end(), [](int a, int b)
	{
		if (oppCards[a].defense == oppCards[b].defense)
		{
			return oppCards[a].attack < oppCards[b].attack;
		}
		return oppCards[a].defense < oppCards[b].defense;
	});

	for (int i = 0; i < guards.size(); i++)
	{
		int id = getBestAttacker(guards[i]);
		if (id < 0)
			id = 0;

		if (id == i)
			continue;
		swap(myCards[id], myCards[i]);
	}

	for (int i = 0; i < myCards.size(); i++)
	{
		int id = i < guards.size() ? oppCards[guards[i]].id : -1;
		commands.emplace_back(getAttackCommand(myCards[i], id));
	}
}

void getCommands(vector<string>& commands)
{
	getSummonCommands(commands);
	getAttackCommands(commands);
}


int main()
{
	int turn = 0;

	// game loop
	for (;; turn++)
	{
		myHand.clear();
		myCards.clear();
		oppCards.clear();
		for (int i = 0; i < 2; i++)
		{
			int playerHealth;
			int playerMana;
			int playerDeck;
			int playerRune;
			cin >> players[i].health >> players[i].maxMana >> players[i].sizeDeck >> players[i].rune; cin.ignore();
			cerr << players[i].health << " " << players[i].maxMana << " " << players[i].sizeDeck << " " << players[i].rune << endl;
			//cout << "cardcpp " << players[i].health << " " << players[i].maxMana << " " << players[i].sizeDeck << " " << players[i].rune << endl;
			//cerr << "cardcpp error stream" << i << endl;
		}
		int opponentHand;
		cin >> players[1].sizeHand; cin.ignore();
		cerr << players[1].sizeHand << endl;
		int cardCount;
		cin >> cardCount; cin.ignore();
		cerr << cardCount << endl;
		for (int i = 0; i < cardCount; i++)
		{
			Card card;
			int cardNumber;
			int instanceId;
			int location;
			int cardType;
			int cost;
			int attack;
			int defense;
			string abilities;
			int myHealthChange;
			int opponentHealthChange;
			int cardDraw;
			cin >> card.number >> card.id >> card.location >> card.type >> card.cost >> card.attack >> card.defense >> card.abils >> card.myHealthChange >> card.oppHealthChange >> card.draw; cin.ignore();
			cerr << card.number << " " << card.id << " " << card.location << " " << card.type << " " << card.cost << " " << card.attack << " " << card.defense << " " << card.abils << " " << card.myHealthChange << " " << card.oppHealthChange << " " << card.draw; cin.ignore();
			if (card.location == 0)
				myHand.emplace_back(card);
			else if (card.location == 1)
				myCards.emplace_back(card);
			else if (card.location == -1)
				oppCards.emplace_back(card);
		}

		players[0].sizeHand = myHand.size();

		sortMyHand();

		vector<string> commands;
		if (isDraft(turn))
			commands.emplace_back(draftCards(myHand));
		else
			getCommands(commands);

		cout << "PASS" << endl << flush;
		//printCommands(commands);
	}
}

