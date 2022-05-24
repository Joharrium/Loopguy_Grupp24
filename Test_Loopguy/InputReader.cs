using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum ControllerMode
{
	Controller, Keyboard
}
static class InputReader
{
	public static KeyboardState keyState, oldKeyState = Keyboard.GetState();
	public static MouseState mouseState, oldMouseState = Mouse.GetState();
	public static GamePadState padState, oldPadState = GamePad.GetState(PlayerIndex.One);

	public static ControllerMode controllerMode = ControllerMode.Controller;

	public static bool editMode;
	public static bool playerInputEnabled = true;

	private static void SetControllerUsed(bool check)
    {
		if(check)
        {
			controllerMode = ControllerMode.Controller;
        }
    }

	private static void SetMouseKeyboardUsed(bool check)
    {
		if(check)
        {
			controllerMode = ControllerMode.Keyboard;
        }
    }

	public static bool KeyPressed(Keys key)
	{
		if (playerInputEnabled)
		{
			SetMouseKeyboardUsed(keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key));
			return keyState.IsKeyDown(key) && oldKeyState.IsKeyUp(key);
		}
		else
			return false;
	}
	public static bool ButtonPressed(Buttons button)
	{
		if (playerInputEnabled)
        {
			SetControllerUsed(padState.IsButtonDown(button) && oldPadState.IsButtonUp(button));
			return padState.IsButtonDown(button) && oldPadState.IsButtonUp(button);
		}
			
		else
			return false;
	}
	public static bool LeftClick()
	{
		if (playerInputEnabled)
        {
			SetMouseKeyboardUsed(mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released);
			return mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;
		}
			
		else
			return false;
	}
	public static bool RightClick()
	{
		if (playerInputEnabled)
		{
			SetMouseKeyboardUsed(mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released);
			return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released;
		}
		else
			return false;
	}
	public static bool MovementInput()
    {
		return MovementLeft() || MovementRight() || MovementUp() || MovementDown() || MovingLeftStick();
    }
	public static bool MovementLeft()
	{
		if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D) || padState.IsButtonDown(Buttons.DPadRight))
			return false;

		if (playerInputEnabled)
			return keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A) || padState.IsButtonDown(Buttons.DPadLeft);
		else
			return false;
	}
	public static bool MovementRight()
	{
		if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A) || padState.IsButtonDown(Buttons.DPadLeft))
			return false;

		if (playerInputEnabled)
			return keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D) || padState.IsButtonDown(Buttons.DPadRight);
		else
			return false;
	}
	public static bool MovementUp()
	{
		if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S) || padState.IsButtonDown(Buttons.DPadDown))
			return false;

		if (playerInputEnabled)
			return keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W) || padState.IsButtonDown(Buttons.DPadUp);
		else
			return false;
	}
	public static bool MovementDown()
	{
		if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W) || padState.IsButtonDown(Buttons.DPadUp))
			return false;

		if (playerInputEnabled)
			return keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S) || padState.IsButtonDown(Buttons.DPadDown);
		else
			return false;
	}
	public static bool MovementDownNonContinous()
	{
		if (playerInputEnabled)
		{
			if (keyState.IsKeyDown(Keys.Down) && oldKeyState.IsKeyUp(Keys.Down) || keyState.IsKeyDown(Keys.S) && oldKeyState.IsKeyUp(Keys.S) || padState.IsButtonDown(Buttons.DPadDown) && oldPadState.IsButtonUp(Buttons.DPadDown))        
				return keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S) || padState.IsButtonDown(Buttons.DPadDown);
		}
		return false;
	}
	public static bool MovementUpNonContinous()
	{
		if (playerInputEnabled)
		{
			if (keyState.IsKeyDown(Keys.Up) && oldKeyState.IsKeyUp(Keys.Up) || keyState.IsKeyDown(Keys.W) && oldKeyState.IsKeyUp(Keys.W) || padState.IsButtonDown(Buttons.DPadUp) && oldPadState.IsButtonUp(Buttons.DPadUp))
				return keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W) || padState.IsButtonDown(Buttons.DPadUp);
		}
		return false;
	}

	public static bool MovementLeftNonContinous()
	{
		if (playerInputEnabled)
		{
			if (keyState.IsKeyDown(Keys.Left) && oldKeyState.IsKeyUp(Keys.Left) || keyState.IsKeyDown(Keys.A) && oldKeyState.IsKeyUp(Keys.A) || padState.IsButtonDown(Buttons.DPadLeft) && oldPadState.IsButtonUp(Buttons.DPadLeft))
				return keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A) || padState.IsButtonDown(Buttons.DPadLeft);
		}
		return false;
	}
	public static bool MovementRightNonContinous()
	{
		if (playerInputEnabled)
		{
			if (keyState.IsKeyDown(Keys.Right) && oldKeyState.IsKeyUp(Keys.Right) || keyState.IsKeyDown(Keys.D) && oldKeyState.IsKeyUp(Keys.D) || padState.IsButtonDown(Buttons.DPadRight) && oldPadState.IsButtonUp(Buttons.DPadRight))
				return keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D) || padState.IsButtonDown(Buttons.DPadRight);
		}
		return false;
	}

	public static bool Dash()
    {
		if (playerInputEnabled)
			return keyState.IsKeyDown(Keys.Space) || padState.IsButtonDown(Buttons.A);
		else
			return false;
    }
	public static bool Attack()
    {
		if (editMode || !playerInputEnabled)
			return false;
		else
			return LeftClick() || KeyPressed(Keys.RightControl) || ButtonPressed(Buttons.X);
    }
	public static bool Aim()
    {
		if (playerInputEnabled)
			return keyState.IsKeyDown(Keys.LeftShift) || padState.IsButtonDown(Buttons.LeftTrigger);
		else
			return false;
	}
	public static bool Shoot()
    { //This is same as Attack so should probs just remove
	  //No it's not
		if (playerInputEnabled)
			return LeftClick() || KeyPressed(Keys.RightControl) || ButtonPressed(Buttons.RightTrigger);
		else
			return false;
	}
	public static bool SwitchGun()
    {
		if (playerInputEnabled)
			return KeyPressed(Keys.Q) || ButtonPressed(Buttons.LeftShoulder) || ButtonPressed(Buttons.RightShoulder);
		else
			return false;
    }

	public static bool MovingLeftStick()
	{
		if (playerInputEnabled)
        {
			SetControllerUsed(padState.ThumbSticks.Left != Vector2.Zero);
			return padState.ThumbSticks.Left != Vector2.Zero;
		}
		else
			return false;
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