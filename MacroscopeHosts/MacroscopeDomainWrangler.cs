﻿/*

	This file is part of SEOMacroscope.

	Copyright 2017 Jason Holland.

	The GitHub repository may be found at:

		https://github.com/nazuke/SEOMacroscope

	Foobar is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Foobar is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;

// TODO: this does not work

namespace SEOMacroscope
{

	/// <summary>
	/// This class attempts to determine if two domains are within the same domain space.
	/// </summary>

	public class MacroscopeDomainWrangler : Macroscope
	{

		/**************************************************************************/

		const int Tolerance = 2;

		/**************************************************************************/

		public MacroscopeDomainWrangler ()
		{
			SuppressDebugMsg = false;
		}

		/**************************************************************************/

		public Boolean IsWithinSameDomain ( string sDomainLeft, string sDomainRight )
		{
			return( this.IsWithinSameDomain( sDomainLeft, sDomainRight, Tolerance ) );
		}

		/**************************************************************************/

		public Boolean IsWithinSameDomain ( string DomainLeft, string DomainRight, int AcceptableTolerance )
		{

			Boolean IsWithinSameDomain = false;
			int iScore = 0;

			string[] DomainShort;
			string[] DomainLong;

			string DomainLeftReversed = MacroscopeStringTools.ReverseString( DomainLeft );
			string DomainRightReversed = MacroscopeStringTools.ReverseString( DomainRight );

			int ScoreThreshold = AcceptableTolerance;

			if( Tolerance > 0 ) {
				ScoreThreshold = Tolerance;
			}

			if( DomainLeftReversed.Length > DomainRightReversed.Length ) {
				DomainShort = DomainRightReversed.Split( '.' );
				DomainLong = DomainLeftReversed.Split( '.' );
			} else {
				DomainShort = DomainLeftReversed.Split( '.' );
				DomainLong = DomainRightReversed.Split( '.' );
			}

			DomainShort = MacroscopeStringTools.ReverseStringArray( DomainShort );
			DomainLong = MacroscopeStringTools.ReverseStringArray( DomainLong );

			DebugMsg( string.Format( "DomainShort: {0} :: {1}", DomainShort.Length, string.Join( " ", DomainShort ) ) );
			DebugMsg( string.Format( "DomainLong: {0} :: {1}", DomainLong.Length, string.Join( " ", DomainLong ) ) );

			DebugMsg( "" );

			for( int i = 0; i < DomainShort.Length; i++ ) {

				if( DomainShort[ i ] == DomainLong[ i ] ) {
					iScore++;
				} else {
					break;
				}

				DebugMsg(
					string.Format(
						"MATCH CHECK: {0} :: {1} :: {2} :: {3}",
						i,
						IsWithinSameDomain,
						DomainShort[ i ], DomainLong[ i ]
					)
				);

			}

			DebugMsg( string.Format( "SCORE: {0}", iScore ) );

			DebugMsg( string.Format( "DomainShort.Length: {0}", DomainShort.Length ) );

			if( DomainShort.Length >= 3 ) {
				if( iScore >= ScoreThreshold ) {
					IsWithinSameDomain = true;
				}
			}

			DebugMsg( string.Format( "bIsWithinSameDomain: {0}", IsWithinSameDomain ) );

			DebugMsg( "" );

			return( IsWithinSameDomain );

		}

		/**************************************************************************/

	}

}
