﻿using System.Diagnostics;
using System.Runtime.CompilerServices;
using Uno.UI.Xaml;

namespace Windows.UI.Xaml
{
	[DebuggerDisplay("{" + nameof(Name) + "}")]
	public partial class RoutedEvent
	{
		public RoutedEvent([CallerMemberName] string name = null)
			: this(RoutedEventFlag.None, name)
		{
		}

		internal RoutedEvent(
			RoutedEventFlag flag,
			[CallerMemberName] string name = null)
		{
			Flag = flag;
			Name = name;

			IsPointerEvent = flag.IsPointerEvent();
			IsGestureEvent = flag.IsGestureEvent();
			IsKeyEvent = flag.IsKeyEvent();
			IsFocusEvent = flag.IsFocusEvent();
		}

		internal string Name { get; }

		internal RoutedEventFlag Flag { get; }

		internal bool IsPointerEvent { get; }
		internal bool IsGestureEvent { get; }
		internal bool IsKeyEvent { get; }
		internal bool IsFocusEvent { get; }
	}
}
