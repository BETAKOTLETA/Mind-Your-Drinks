using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Mind_Your_Drink_Models.Models;

public class CircularProgressDrawable : IDrawable
{
    public float Progress { get; set; }  
    const float Stroke = 12f;           
    const float FontSize = 24f;         

    public void Draw(ICanvas canvas, RectF rect)
    {

        float displayProgress = Math.Min(Progress, 20f);


        float radius = Math.Min(rect.Width, rect.Height) / 2 - Stroke;
        float centerX = rect.Center.X;
        float centerY = rect.Center.Y;

        /* ---------- BACKGROUND TRACK ---------- */
        canvas.StrokeSize = Stroke;
        canvas.StrokeLineCap = LineCap.Butt;
        canvas.StrokeColor = Colors.LightGray;
        canvas.DrawCircle(centerX, centerY, radius);

        /* ---------- PROGRESS ARC ---------- */
        if (displayProgress > 0f)
        {
            canvas.StrokeColor = Mix(displayProgress);

            if (displayProgress >= 1f)
            {
                canvas.DrawCircle(centerX, centerY, radius);
            }
            else
            {
                canvas.StrokeLineCap = LineCap.Round;
                float startAngle = 0f;               
                float sweepAngle = 360f * displayProgress;

                canvas.DrawArc(
                    centerX - radius,
                    centerY - radius,
                    radius * 2,
                    radius * 2,
                    startAngle,
                    sweepAngle,
                    false,  // Clockwise
                    false
                );
            }
        }

        string labelText = $"{(int)(displayProgress * 100)}%";
        canvas.Font = Microsoft.Maui.Graphics.Font.Default;
        canvas.FontSize = FontSize;
        canvas.FontColor = Mix(displayProgress);

        var textSize = canvas.GetStringSize(labelText, Microsoft.Maui.Graphics.Font.Default, FontSize);
        canvas.DrawString(
            labelText,
            centerX - textSize.Width / 2,
            centerY - textSize.Height / 2,
            textSize.Width,
            textSize.Height,
            HorizontalAlignment.Center,
            VerticalAlignment.Center
        );
    }

    /* ---------- COLOR GRADIENT HELPERS ---------- */
    static Microsoft.Maui.Graphics.Color Lerp(Microsoft.Maui.Graphics.Color a, Microsoft.Maui.Graphics.Color b, float t) =>
        new Microsoft.Maui.Graphics.Color(
            a.Red + (b.Red - a.Red) * t,
            a.Green + (b.Green - a.Green) * t,
            a.Blue + (b.Blue - a.Blue) * t,
            1f
        );

    static Microsoft.Maui.Graphics.Color Mix(float progress)  // Green → Yellow → Red
    {
        // Ensure progress is clamped for color calculation
        float clamped = Math.Min(1f, Math.Max(0f, progress));
        return clamped < 0.5f
            ? Lerp(Microsoft.Maui.Graphics.Colors.LimeGreen,
                   Microsoft.Maui.Graphics.Colors.Gold,
                   clamped / 0.5f)
            : Lerp(Microsoft.Maui.Graphics.Colors.Gold,
                   Microsoft.Maui.Graphics.Colors.Red,
                   (clamped - 0.5f) / 0.5f);
    }
}

