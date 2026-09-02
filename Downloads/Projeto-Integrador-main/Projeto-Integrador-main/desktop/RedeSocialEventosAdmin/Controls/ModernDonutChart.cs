using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace RedeSocialEventosAdmin.Controls
{
  public class ModernDonutChart : Control
  {
    public class Slice
    {
      public string Label { get; set; } = string.Empty;
      public double Value { get; set; }
      public Color Color { get; set; }
    }

    private readonly List<Slice> _slices = new List<Slice>();
    private static readonly Color[] Palette = new Color[]
    {
      Color.FromArgb(99, 102, 241),  // Indigo (#6366F1)
      Color.FromArgb(16, 185, 129),  // Emerald (#10B981)
      Color.FromArgb(234, 63, 116),  // Rose/Pink (#EA3F74)
      Color.FromArgb(245, 158, 11),  // Amber (#F59E0B)
      Color.FromArgb(14, 165, 233),  // Sky (#0EA5E9)
      Color.FromArgb(168, 85, 247),  // Purple (#A855F7)
      Color.FromArgb(100, 116, 139)  // Slate (#64748B)
    };

    public string CenterTitle { get; set; } = "Total";
    public string CenterSubtitle { get; set; } = "Distribuição";

    public ModernDonutChart()
    {
      this.DoubleBuffered = true;
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
      this.BackColor = Color.White;
      this.Font = new Font("Segoe UI", 9F);
    }

    public void SetData(IEnumerable<Tuple<string, int>> data)
    {
      _slices.Clear();
      int colorIndex = 0;
      foreach (var item in data)
      {
        if (item.Item2 > 0)
        {
          _slices.Add(new Slice
          {
            Label = item.Item1,
            Value = item.Item2,
            Color = Palette[colorIndex % Palette.Length]
          });
          colorIndex++;
        }
      }
      this.Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      Graphics g = e.Graphics;
      g.SmoothingMode = SmoothingMode.AntiAlias;
      g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

      // Background & Border
      using (GraphicsPath path = GetRoundedRect(new Rectangle(0, 0, Width - 1, Height - 1), 12))
      {
        using (SolidBrush bg = new SolidBrush(BackColor))
        {
          g.FillPath(bg, path);
        }
        using (Pen borderPen = new Pen(Color.FromArgb(226, 232, 240), 1))
        {
          g.DrawPath(borderPen, path);
        }
      }

      if (_slices.Count == 0)
      {
        using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
        {
          g.DrawString("Sem dados para exibição", Font, new SolidBrush(Color.FromArgb(148, 163, 184)), new Rectangle(0, 0, Width, Height), sf);
        }
        return;
      }

      double total = _slices.Sum(s => s.Value);
      int chartDiameter = Math.Min(Width / 2 - 30, Height - 40);
      int chartX = 20;
      int chartY = (Height - chartDiameter) / 2;
      Rectangle chartRect = new Rectangle(chartX, chartY, chartDiameter, chartDiameter);

      float startAngle = -90f;
      foreach (var slice in _slices)
      {
        float sweepAngle = (float)((slice.Value / total) * 360f);
        using (SolidBrush brush = new SolidBrush(slice.Color))
        {
          g.FillPie(brush, chartRect, startAngle, sweepAngle);
        }
        startAngle += sweepAngle;
      }

      // Donut Hole (Center)
      int holeDiameter = (int)(chartDiameter * 0.62);
      int holeX = chartX + (chartDiameter - holeDiameter) / 2;
      int holeY = chartY + (chartDiameter - holeDiameter) / 2;
      Rectangle holeRect = new Rectangle(holeX, holeY, holeDiameter, holeDiameter);

      using (SolidBrush holeBrush = new SolidBrush(BackColor))
      {
        g.FillEllipse(holeBrush, holeRect);
      }

      // Center Text
      using (Font titleFont = new Font("Segoe UI", 12F, FontStyle.Bold))
      using (Font subFont = new Font("Segoe UI", 8F))
      using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
      {
        Rectangle textRect = new Rectangle(holeX, holeY - 4, holeDiameter, holeDiameter);
        g.DrawString(total.ToString("N0"), titleFont, new SolidBrush(Color.FromArgb(15, 23, 42)), new RectangleF(holeX, holeY + (holeDiameter / 4) - 8, holeDiameter, 20), sf);
        g.DrawString(CenterSubtitle, subFont, new SolidBrush(Color.FromArgb(148, 163, 184)), new RectangleF(holeX, holeY + (holeDiameter / 2), holeDiameter, 16), sf);
      }

      // Legend on the Right
      int legendX = chartX + chartDiameter + 25;
      int legendY = 25;
      int legendSpacing = Math.Max(20, (Height - 50) / Math.Max(1, _slices.Count));

      using (Font legendFont = new Font("Segoe UI", 8.5F, FontStyle.Bold))
      using (Font valFont = new Font("Segoe UI", 8F))
      {
        for (int i = 0; i < _slices.Count; i++)
        {
          var slice = _slices[i];
          int curY = legendY + (i * legendSpacing);

          // Legend Circle Indicator
          using (SolidBrush indBrush = new SolidBrush(slice.Color))
          {
            g.FillEllipse(indBrush, legendX, curY + 2, 10, 10);
          }

          // Percentage
          double pct = (slice.Value / total) * 100.0;
          string labelText = $"{slice.Label}";
          string valText = $"{slice.Value:N0} ({pct:F1}%)";

          g.DrawString(labelText, legendFont, new SolidBrush(Color.FromArgb(30, 41, 59)), legendX + 16, curY);
          g.DrawString(valText, valFont, new SolidBrush(Color.FromArgb(100, 116, 139)), legendX + 16, curY + 14);
        }
      }
    }

    private static GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
    {
      GraphicsPath path = new GraphicsPath();
      int diameter = radius * 2;
      Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

      path.AddArc(arc, 180, 90);
      arc.X = bounds.Right - diameter;
      path.AddArc(arc, 270, 90);
      arc.Y = bounds.Bottom - diameter;
      path.AddArc(arc, 0, 90);
      arc.X = bounds.Left;
      path.AddArc(arc, 90, 90);
      path.CloseFigure();
      return path;
    }
  }
}