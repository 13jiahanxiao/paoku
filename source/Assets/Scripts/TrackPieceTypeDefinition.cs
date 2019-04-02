using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TrackPieceTypeCategory
{
	kTrackPieceCategoryNormal = 0,
	kTrackPieceCategoryCompoundPieceStart,
	kTrackPieceCategoryCompoundPieceMiddle,
	kTrackPieceCategoryCompoundPieceEnd,
	kTrackPieceCategoryEnvironmentSetTransitionStart,
	kTrackPieceCategoryEnvironmentSetTransitionEnd,
	kTrackPieceCategoryEnvironmentTransitionStart,
	kTrackPieceCategoryEnvironmentTransitionEnd,
	kTrackPieceMaxx
}

//TODO: Transition to this! that way we dont have a gazillion bools lying around...
//also allows "Is[Flag]() functions in TrackBuilder to be faster and less space-wasting
public enum TrackPieceTypeFlag
{
	kIsTurnLeft = 1,
	kIsTurnRight = 2,
	kIsJunction = 4,
	kIsSlideUnder = 8,
	kIsJumpOver = 16,
	kIsStumble = 32,
	kIsLedgeLeft = 64,
	kIsLedgeRight = 128,
	kIsTransitionTunnel = 256,
	kIsStairs = 512,
	kIsCurve = 1024,
	kIsMine = 2048,
	kIsZipLine = 4096,
	kIsAnimated = 8192,
	kIsEnemy = 16386,
}

[System.Serializable]
public class TrackPieceTypeDefinition : System.Object
{
	public TrackPiece.PieceType TrackType = TrackPiece.PieceType.kTPMaxx;
	public int EnvironmentSet = 0;
	public int Environment = 0;
	public TrackPieceTypeCategory TrackCategory = TrackPieceTypeCategory.kTrackPieceMaxx;
	public bool IsTurnLeft = false;
	public bool IsTurnRight = false;
	public bool IsSlightLeft = false;
	public bool IsSlightRight = false;
	public bool IsJunction = false;
	public TrackPiece.PieceType AfterJunctionPiece = TrackPiece.PieceType.kTPStraightFlat;
	public TrackPiece.PieceType AlternateAfterJunctionPiece = TrackPiece.PieceType.kTPBalloonEntrance;
	public bool IsSlideUnder = false;
	public bool IsJumpOver = false;
	public bool IsAttackingBaboon = false;
	public bool IsGap = false;
	public bool IsStumble = false;
	public bool IsLedgeLeft = false;
	public bool IsLedgeRight = false;
	public bool IsTransitionTunnel = false;
	public bool IsStairs = false;
	public bool IsCurve = false;
	public bool IsMine = false;
	public bool IsBalloon = false;
	public bool IsBalloonJunction = false;
	public bool IsCloudsJunction = false;
	public bool IsZipLine = false;
	public bool IsNarrow = false;
	public bool IsAnimated = false;
	public bool IsEnemy = false;	
	public bool IsCoinBlock = false;
	/// <summary>
	/// do we normally allow this piece in TrackBuilder.ChooseNextTrackPiece?, defaults to true
	/// </summary>
	public bool AllowInRegularRotation = true; 
	/// <summary>
	/// do we allow this piece when fast travelling? defaults to true
	/// </summary>
	public bool AllowWhenFastTravelling = true;
	public TrackPiece.PieceType CompoundPieceMiddleType = TrackPiece.PieceType.kTPMaxx;
	public TrackPiece.PieceType CompoundPieceEndType = TrackPiece.PieceType.kTPMaxx;
	public int CompoundPieceMinMiddleCount = 1;
	public int CompoundPieceMaxMiddleCount = 1;
	public int DifficultyLevel = 0;
	public float SelectionOdds = 1.0f;
	public List<TrackPiece.PieceType> PrePieces = new List<TrackPiece.PieceType>();		// Pieces to always pre-append to this piece
	public List<TrackPiece.PieceType> PostPieces = new List<TrackPiece.PieceType>();	// Pieces to always post-append to this piece
	public Dictionary<string, float> Variations = new Dictionary<string, float>();
	public int LastChosenVariation = 0;
	
	
	public void AddVariation(string key, float frequency=0.0f)
	{
		if(Variations.ContainsKey(key) == true) {
			Variations[key] = frequency;
		}
		else if(frequency < Mathf.Epsilon) {
			Variations.Add (key, frequency);
			frequency = 1.0f / (float)(Variations.Count);
			
			//-- set all pieces to be equal
			List<string> keys = new List<string>(Variations.Keys);
			foreach (string item in keys) {
				Variations[item] = frequency;
			}
			//TR.LOG("key={0}, f={1}, d={2}", key, frequency, Variations);
		}
		else {
			Variations.Add (key, frequency);
		}
	}
	
	public int GetVariation(int prevChoice, ref string choiceString) {
		
		List<string> keys = new List<string>(Variations.Keys);
		int choice = prevChoice;
		
		if(keys.Count == 1) {
			choice = 0;
			choiceString = keys[choice];
			return choice;
		}
		
		//int rndChoice = 0;
		//float freq = 0.0f;
		List<float> values = new List<float>(Variations.Values);
		
		choice = Random.Range(0,values.Count);
		
		/*while(prevChoice == choice) {
			rndChoice = Random.Range(0, 100);
			freq = 0.0f;
			for(int i=0; i<values.Count; i++) {
				freq += (values[i] * 100.0f);
				if(rndChoice < (int)freq) {
					choice = i;
					break;
				}
			}	
		}*/
		
		choiceString = keys[choice];
		return choice;
	}
}

