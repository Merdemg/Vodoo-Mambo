#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_EVT_ACHVMNT : GSTypedRequest<LogEventRequest_EVT_ACHVMNT, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_ACHVMNT() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_ACHVMNT");
		}
		
		public LogEventRequest_EVT_ACHVMNT Set_ACHVMNT( string value )
		{
			request.AddString("ACHVMNT", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_EVT_ACHVMNT : GSTypedRequest<LogChallengeEventRequest_EVT_ACHVMNT, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_ACHVMNT() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_ACHVMNT");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_ACHVMNT SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_ACHVMNT Set_ACHVMNT( string value )
		{
			request.AddString("ACHVMNT", value);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_ADDCOINS : GSTypedRequest<LogEventRequest_EVT_ADDCOINS, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_ADDCOINS() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_ADDCOINS");
		}
		public LogEventRequest_EVT_ADDCOINS Set_AMOUNT( long value )
		{
			request.AddNumber("AMOUNT", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_EVT_ADDCOINS : GSTypedRequest<LogChallengeEventRequest_EVT_ADDCOINS, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_ADDCOINS() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_ADDCOINS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_ADDCOINS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_ADDCOINS Set_AMOUNT( long value )
		{
			request.AddNumber("AMOUNT", value);
			return this;
		}			
	}
	
	public class LogEventRequest_EVT_CHCKMEDAL : GSTypedRequest<LogEventRequest_EVT_CHCKMEDAL, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_CHCKMEDAL() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_CHCKMEDAL");
		}
	}
	
	public class LogChallengeEventRequest_EVT_CHCKMEDAL : GSTypedRequest<LogChallengeEventRequest_EVT_CHCKMEDAL, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_CHCKMEDAL() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_CHCKMEDAL");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_CHCKMEDAL SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_Evt_Ach_First_Play : GSTypedRequest<LogEventRequest_Evt_Ach_First_Play, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_Evt_Ach_First_Play() : base("LogEventRequest"){
			request.AddString("eventKey", "Evt_Ach_First_Play");
		}
	}
	
	public class LogChallengeEventRequest_Evt_Ach_First_Play : GSTypedRequest<LogChallengeEventRequest_Evt_Ach_First_Play, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_Evt_Ach_First_Play() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "Evt_Ach_First_Play");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_Evt_Ach_First_Play SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_GETBALLS : GSTypedRequest<LogEventRequest_EVT_GETBALLS, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_GETBALLS() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_GETBALLS");
		}
	}
	
	public class LogChallengeEventRequest_EVT_GETBALLS : GSTypedRequest<LogChallengeEventRequest_EVT_GETBALLS, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_GETBALLS() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_GETBALLS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_GETBALLS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_GETBESTGLBRANK : GSTypedRequest<LogEventRequest_EVT_GETBESTGLBRANK, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_GETBESTGLBRANK() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_GETBESTGLBRANK");
		}
		
		public LogEventRequest_EVT_GETBESTGLBRANK Set_PLAYERID( string value )
		{
			request.AddString("PLAYERID", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_EVT_GETBESTGLBRANK : GSTypedRequest<LogChallengeEventRequest_EVT_GETBESTGLBRANK, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_GETBESTGLBRANK() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_GETBESTGLBRANK");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_GETBESTGLBRANK SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_GETBESTGLBRANK Set_PLAYERID( string value )
		{
			request.AddString("PLAYERID", value);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_GETFRIENDIDS : GSTypedRequest<LogEventRequest_EVT_GETFRIENDIDS, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_GETFRIENDIDS() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_GETFRIENDIDS");
		}
	}
	
	public class LogChallengeEventRequest_EVT_GETFRIENDIDS : GSTypedRequest<LogChallengeEventRequest_EVT_GETFRIENDIDS, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_GETFRIENDIDS() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_GETFRIENDIDS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_GETFRIENDIDS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_GETMEDALCTID : GSTypedRequest<LogEventRequest_EVT_GETMEDALCTID, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_GETMEDALCTID() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_GETMEDALCTID");
		}
		
		public LogEventRequest_EVT_GETMEDALCTID Set_PLAYERID( string value )
		{
			request.AddString("PLAYERID", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_EVT_GETMEDALCTID : GSTypedRequest<LogChallengeEventRequest_EVT_GETMEDALCTID, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_GETMEDALCTID() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_GETMEDALCTID");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_GETMEDALCTID SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_GETMEDALCTID Set_PLAYERID( string value )
		{
			request.AddString("PLAYERID", value);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_GETACHVMNTSID : GSTypedRequest<LogEventRequest_EVT_GETACHVMNTSID, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_GETACHVMNTSID() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_GETACHVMNTSID");
		}
		
		public LogEventRequest_EVT_GETACHVMNTSID Set_player_id( string value )
		{
			request.AddString("player_id", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_EVT_GETACHVMNTSID : GSTypedRequest<LogChallengeEventRequest_EVT_GETACHVMNTSID, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_GETACHVMNTSID() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_GETACHVMNTSID");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_GETACHVMNTSID SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_GETACHVMNTSID Set_player_id( string value )
		{
			request.AddString("player_id", value);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_GETTIME : GSTypedRequest<LogEventRequest_EVT_GETTIME, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_GETTIME() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_GETTIME");
		}
	}
	
	public class LogChallengeEventRequest_EVT_GETTIME : GSTypedRequest<LogChallengeEventRequest_EVT_GETTIME, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_GETTIME() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_GETTIME");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_GETTIME SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_MOVESCORE : GSTypedRequest<LogEventRequest_EVT_MOVESCORE, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_MOVESCORE() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_MOVESCORE");
		}
		public LogEventRequest_EVT_MOVESCORE Set_SCORE( long value )
		{
			request.AddNumber("SCORE", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_EVT_MOVESCORE : GSTypedRequest<LogChallengeEventRequest_EVT_MOVESCORE, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_MOVESCORE() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_MOVESCORE");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_MOVESCORE SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_MOVESCORE Set_SCORE( long value )
		{
			request.AddNumber("SCORE", value);
			return this;
		}			
	}
	
	public class LogEventRequest_EVT_RMVCOINS : GSTypedRequest<LogEventRequest_EVT_RMVCOINS, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_RMVCOINS() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_RMVCOINS");
		}
		public LogEventRequest_EVT_RMVCOINS Set_AMOUNT( long value )
		{
			request.AddNumber("AMOUNT", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_EVT_RMVCOINS : GSTypedRequest<LogChallengeEventRequest_EVT_RMVCOINS, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_RMVCOINS() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_RMVCOINS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_RMVCOINS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_RMVCOINS Set_AMOUNT( long value )
		{
			request.AddNumber("AMOUNT", value);
			return this;
		}			
	}
	
	public class LogEventRequest_Evt_Ach_Score : GSTypedRequest<LogEventRequest_Evt_Ach_Score, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_Evt_Ach_Score() : base("LogEventRequest"){
			request.AddString("eventKey", "Evt_Ach_Score");
		}
	}
	
	public class LogChallengeEventRequest_Evt_Ach_Score : GSTypedRequest<LogChallengeEventRequest_Evt_Ach_Score, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_Evt_Ach_Score() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "Evt_Ach_Score");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_Evt_Ach_Score SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_SCORE_EVT : GSTypedRequest<LogEventRequest_SCORE_EVT, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_SCORE_EVT() : base("LogEventRequest"){
			request.AddString("eventKey", "SCORE_EVT");
		}
		public LogEventRequest_SCORE_EVT Set_SCORE_ATTR( long value )
		{
			request.AddNumber("SCORE_ATTR", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_SCORE_EVT : GSTypedRequest<LogChallengeEventRequest_SCORE_EVT, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_SCORE_EVT() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "SCORE_EVT");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_SCORE_EVT SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_SCORE_EVT Set_SCORE_ATTR( long value )
		{
			request.AddNumber("SCORE_ATTR", value);
			return this;
		}			
	}
	
	public class LogEventRequest_EVT_SETBALLS : GSTypedRequest<LogEventRequest_EVT_SETBALLS, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_SETBALLS() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_SETBALLS");
		}
		public LogEventRequest_EVT_SETBALLS Set_LVL0( long value )
		{
			request.AddNumber("LVL0", value);
			return this;
		}			
		public LogEventRequest_EVT_SETBALLS Set_LVL1( long value )
		{
			request.AddNumber("LVL1", value);
			return this;
		}			
		public LogEventRequest_EVT_SETBALLS Set_LVL2( long value )
		{
			request.AddNumber("LVL2", value);
			return this;
		}			
		public LogEventRequest_EVT_SETBALLS Set_LVL3( long value )
		{
			request.AddNumber("LVL3", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_EVT_SETBALLS : GSTypedRequest<LogChallengeEventRequest_EVT_SETBALLS, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_SETBALLS() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_SETBALLS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_SETBALLS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_SETBALLS Set_LVL0( long value )
		{
			request.AddNumber("LVL0", value);
			return this;
		}			
		public LogChallengeEventRequest_EVT_SETBALLS Set_LVL1( long value )
		{
			request.AddNumber("LVL1", value);
			return this;
		}			
		public LogChallengeEventRequest_EVT_SETBALLS Set_LVL2( long value )
		{
			request.AddNumber("LVL2", value);
			return this;
		}			
		public LogChallengeEventRequest_EVT_SETBALLS Set_LVL3( long value )
		{
			request.AddNumber("LVL3", value);
			return this;
		}			
	}
	
	public class LogEventRequest_EVT_SETCOINS : GSTypedRequest<LogEventRequest_EVT_SETCOINS, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_SETCOINS() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_SETCOINS");
		}
		public LogEventRequest_EVT_SETCOINS Set_AMOUNT( long value )
		{
			request.AddNumber("AMOUNT", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_EVT_SETCOINS : GSTypedRequest<LogChallengeEventRequest_EVT_SETCOINS, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_SETCOINS() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_SETCOINS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_SETCOINS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_EVT_SETCOINS Set_AMOUNT( long value )
		{
			request.AddNumber("AMOUNT", value);
			return this;
		}			
	}
	
	public class LogEventRequest_TEST_GETCOINS : GSTypedRequest<LogEventRequest_TEST_GETCOINS, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_TEST_GETCOINS() : base("LogEventRequest"){
			request.AddString("eventKey", "TEST_GETCOINS");
		}
	}
	
	public class LogChallengeEventRequest_TEST_GETCOINS : GSTypedRequest<LogChallengeEventRequest_TEST_GETCOINS, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_TEST_GETCOINS() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "TEST_GETCOINS");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_TEST_GETCOINS SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_TEST_DAILY : GSTypedRequest<LogEventRequest_TEST_DAILY, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_TEST_DAILY() : base("LogEventRequest"){
			request.AddString("eventKey", "TEST_DAILY");
		}
	}
	
	public class LogChallengeEventRequest_TEST_DAILY : GSTypedRequest<LogChallengeEventRequest_TEST_DAILY, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_TEST_DAILY() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "TEST_DAILY");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_TEST_DAILY SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_EVT_TESTPUSH : GSTypedRequest<LogEventRequest_EVT_TESTPUSH, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_EVT_TESTPUSH() : base("LogEventRequest"){
			request.AddString("eventKey", "EVT_TESTPUSH");
		}
	}
	
	public class LogChallengeEventRequest_EVT_TESTPUSH : GSTypedRequest<LogChallengeEventRequest_EVT_TESTPUSH, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_EVT_TESTPUSH() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "EVT_TESTPUSH");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_EVT_TESTPUSH SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_TEST : GSTypedRequest<LogEventRequest_TEST, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_TEST() : base("LogEventRequest"){
			request.AddString("eventKey", "TEST");
		}
	}
	
	public class LogChallengeEventRequest_TEST : GSTypedRequest<LogChallengeEventRequest_TEST, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_TEST() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "TEST");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_TEST SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
}
	
	
	
namespace GameSparks.Api.Requests{
	
	public class LeaderboardDataRequest_HIGH_SCORE_LB : GSTypedRequest<LeaderboardDataRequest_HIGH_SCORE_LB,LeaderboardDataResponse_HIGH_SCORE_LB>
	{
		public LeaderboardDataRequest_HIGH_SCORE_LB() : base("LeaderboardDataRequest"){
			request.AddString("leaderboardShortCode", "HIGH_SCORE_LB");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LeaderboardDataResponse_HIGH_SCORE_LB (response);
		}		
		
		/// <summary>
		/// The challenge instance to get the leaderboard data for
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// The offset into the set of leaderboards returned
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetOffset( long offset )
		{
			request.AddNumber("offset", offset);
			return this;
		}
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public LeaderboardDataRequest_HIGH_SCORE_LB SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
		
	}

	public class AroundMeLeaderboardRequest_HIGH_SCORE_LB : GSTypedRequest<AroundMeLeaderboardRequest_HIGH_SCORE_LB,AroundMeLeaderboardResponse_HIGH_SCORE_LB>
	{
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB() : base("AroundMeLeaderboardRequest"){
			request.AddString("leaderboardShortCode", "HIGH_SCORE_LB");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new AroundMeLeaderboardResponse_HIGH_SCORE_LB (response);
		}		
		
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_HIGH_SCORE_LB SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
	}
}

namespace GameSparks.Api.Responses{
	
	public class _LeaderboardEntry_HIGH_SCORE_LB : LeaderboardDataResponse._LeaderboardData{
		public _LeaderboardEntry_HIGH_SCORE_LB(GSData data) : base(data){}
		public long? SCORE_ATTR{
			get{return response.GetNumber("SCORE_ATTR");}
		}
	}
	
	public class LeaderboardDataResponse_HIGH_SCORE_LB : LeaderboardDataResponse
	{
		public LeaderboardDataResponse_HIGH_SCORE_LB(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> Data_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> First_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> Last_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
	}
	
	public class AroundMeLeaderboardResponse_HIGH_SCORE_LB : AroundMeLeaderboardResponse
	{
		public AroundMeLeaderboardResponse_HIGH_SCORE_LB(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> Data_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> First_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB> Last_HIGH_SCORE_LB{
			get{return new GSEnumerable<_LeaderboardEntry_HIGH_SCORE_LB>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_HIGH_SCORE_LB(data);});}
		}
	}
}	

namespace GameSparks.Api.Messages {

		public class ScriptMessage_PUSH_LBRESET : ScriptMessage {
		
			public new static Action<ScriptMessage_PUSH_LBRESET> Listener;
	
			public ScriptMessage_PUSH_LBRESET(GSData data) : base(data){}
	
			private static ScriptMessage_PUSH_LBRESET Create(GSData data)
			{
				ScriptMessage_PUSH_LBRESET message = new ScriptMessage_PUSH_LBRESET (data);
				return message;
			}
	
			static ScriptMessage_PUSH_LBRESET()
			{
				handlers.Add (".ScriptMessage_PUSH_LBRESET", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}
		public class ScriptMessage_testmsg : ScriptMessage {
		
			public new static Action<ScriptMessage_testmsg> Listener;
	
			public ScriptMessage_testmsg(GSData data) : base(data){}
	
			private static ScriptMessage_testmsg Create(GSData data)
			{
				ScriptMessage_testmsg message = new ScriptMessage_testmsg (data);
				return message;
			}
	
			static ScriptMessage_testmsg()
			{
				handlers.Add (".ScriptMessage_testmsg", Create);
	
			}
			
			override public void NotifyListeners()
			{
				if (Listener != null)
				{
					Listener (this);
				}
			}
		}

}
