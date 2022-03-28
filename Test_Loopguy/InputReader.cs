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

	public static bool Aim()
    {
		return keyState.IsKeyDown(Keys.LeftShift) || padState.IsButtonDown(Buttons.LeftTrigger);
	}

	public static bool Shoot()
    { // Subject to change, might be better to do this in the Player class
		if (Aim())
			return mouseState.LeftButton == ButtonState.Pressed || padState.IsButtonDown(Buttons.RightTrigger);
		else
			return false;
    }

	public static bool MovingLeftStick()
    {
		return padState.ThumbSticks.Left != Vector2.Zero;
    }
	public static Vector2 LeftStickDirection()
    {
		return padState.ThumbSticks.Left;
    }
	public static Vector2 RigthStickDirection()
	{
		return padState.ThumbSticks.Right;
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