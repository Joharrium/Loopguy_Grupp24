using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

static class InputReader
{
	public static KeyboardState keyState, oldKeyState = Keyboard.GetState();
	public static MouseState mouseState, oldMouseState = Mouse.GetState();
	public static GamePadState padState, oldPadState = GamePad.GetState(PlayerIndex.One);

	public static bool editMode;

	public static bool KeyPressed(Keys key)
	{
		return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
	}
	public static bool ButtonPressed(Buttons button)
	{
		return padState.IsButtonDown(button) && oldPadState.IsButtonUp(button);
	}
	public static bool LeftClick()
	{
		return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
	}
	public static bool RightClick()
	{
		return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
	}

	public static bool MovementLeft()
	{
		return keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A) || padState.IsButtonDown(Buttons.DPadLeft);
	}
	public static bool MovementRight()
	{
		return keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D) || padState.IsButtonDown(Buttons.DPadRight);
	}
	public static bool MovementUp()
	{
		return keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W) || padState.IsButtonDown(Buttons.DPadUp);
	}
	public static bool MovementDown()
	{
		return keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S) || padState.IsButtonDown(Buttons.DPadDown);
	}
	
	public static bool Dash()
    {
		return KeyPressed(Keys.Space) || ButtonPressed(Buttons.A);
    }
	public static bool Attack()
    {
		if (editMode)
			return false;
		else
			return LeftClick() || KeyPressed(Keys.RightControl) || ButtonPressed(Buttons.X);
    }
	public static bool Aim()
    {
		return keyState.IsKeyDown(Keys.LeftShift) || padState.IsButtonDown(Buttons.LeftTrigger);
	}
	public static bool Shoot()
    { //This is same as Attack so should probs just remove
		//No it's not
		return LeftClick() || KeyPressed(Keys.RightControl) || ButtonPressed(Buttons.RightTrigger);
	}

	public static bool MovingLeftStick()
    {
		return padState.ThumbSticks.Left != Vector2.Zero;
    }
	public static Vector2 LeftStickDirection()
    {
		return padState.ThumbSticks.Left;
    }
	public static Vector2 RightStickDirection()
	{
		return padState.ThumbSticks.Right;
	}
	public static float LeftStickLength()
	{
		//vectors length is too low!!! About 83% of what it should be when diagonal for some reason
		float length = padState.ThumbSticks.Left.Length();
		if (length > 0) //for some reason the vector seems to be missing (not zero) when thumbstick is not moved. Fucks everything up
			return length;
		else
			return 0;
	}
	public static float LeftStickAngle(float offset)
    {
		float v = (float)Math.Atan2(LeftStickDirection().Y, LeftStickDirection().X) + (float)(Math.PI / 2) + offset;

		if (v < 0.0)
			v += (float)Math.PI * 2;

		return v;
	}
	public static float RightStickAngle(float offset)
    {
		float v = (float)Math.Atan2(RightStickDirection().Y, RightStickDirection().X) + (float)(Math.PI / 2) + offset;

		if (v < 0.0)
			v += (float)Math.PI * 2;

		return v;
	}

	//Should be called at beginning of Update in Game
	public static void Update()
	{
		oldKeyState = keyState;
		keyState = Keyboard.GetState();
		oldMouseState = mouseState;
		mouseState = Mouse.GetState();
		oldPadState = padState;
		padState = GamePad.GetState(PlayerIndex.One);
	}
}