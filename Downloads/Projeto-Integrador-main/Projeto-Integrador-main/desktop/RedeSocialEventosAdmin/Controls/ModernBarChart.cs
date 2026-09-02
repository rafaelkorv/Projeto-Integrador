using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace RedeSocialEventosAdmin.Controls
{
  public class ModernBarChart : Control
  {
    public class BarItem
    {
      public string Title { get; set; } = string.Empty;
      public int Value1 { get; set; }
      public int Value2 { get; set; }
    }

    private readonly List<BarItem> _items = new List<BarItem>();

    public string LabelValue1 { get; set; } = "Inscritos";
    public string LabelValue2 { get; set; } = "Capacidade";
    public Color ColorValue1 { get; set; } = Color.FromArgb(79, 70, 229); // Indigo
    public Color ColorValue2 { get; set; } = Color.FromArgb(203, 213, 225); // Slate Claro

    public ModernBarChart()
    {
      this.DoubleBuffered = true;
      this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
      this.BackColor = Color.White;
      this.Font = new Font("Segoe UI", 9F);
    }

    public void SetData(IEnumerable<Tuple<string, int, int>> data)
    {
      _items.Clear();
      foreach (var d in data)
      {
        _items.Add(new BarItem
        {
          Title = d.Item1,
          Value1 = d.Item2,
          Value2 = d.Item3
        });
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

      if (_items.Count == 0)
      {
        using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
        {
          g.DrawString("Sem dados para exibição", Font, new SolidBrush(Color.FromArgb(148, 163, 184)), new Rectangle(0, 0, Width, Height), sf);
        }
        return;
      }

      // Legend at the Top
      int topY = 15;
      using (Font legendFont = new Font("Segoe UI", 8F, FontStyle.Bold))
      {
        using (SolidBrush b1 = new SolidBrush(ColorValue1))
        {
          g.FillEllipse(b1, Width - 190, topY + 3, 9, 9);
          g.DrawString(LabelValue1, legendFont, new SolidBrush(Color.FromArgb(51, 65, 85)), Width - 175, topY);
        }

        using (SolidBrush b2 = new SolidBrush(ColorValue2))
        {
          g.FillEllipse(b2, Width - 100, topY + 3, 9, 9);
          g.DrawString(LabelValue2, legendFont, new SolidBrush(Color.FromArgb(51, 65, 85)), Width - 85, topY);
        }
      }

      int startY = 42;
      int availableHeight = Height - startY - 15;
      int rowHeight = availableHeight / Math.Max(1, _items.Count);
      int maxVal = Math.Max(1, _items.Max(i => Math.Max(i.Value1, i.Value2)));

      int labelWidth = 140;
      int barMaxWidth = Width - labelWidth - 90;

      using (Font titleFont = new Font("Segoe UI", 8.5F, FontStyle.Bold))
      using (Font badgeFont = new Font("Segoe UI", 7.5F))
      {
        for (int i = 0; i < _items.Count; i++)
        {
          var item = _items[i];
          int curY = startY + (i * rowHeight);

          // Title
          string displayTitle = item.Title.Length > 20 ? item.Title.Substring(0, 18) + "..." : item.Title;
          g.DrawString(displayTitle, titleFont, new SolidBrush(Color.FromArgb(30, 41, 59)), 18, curY + 6);

          // Bar 1 (Foreground Metric)
          int bar1Width = Math.Max(4, (int)(((double)item.Value1 / maxVal) * barMaxWidth));
          int bar1Y = curY + 4;
          int barH = 12;

          // Bar 2 Background (Max/Capacity) if > 0
          if (item.Value2 > 0)
          {
            int bar2Width = Math.Max(4, (int)(((double)item.Value2 / maxVal) * barMaxWidth));
            using (GraphicsPath b2Path = GetRoundedRect(new Rectangle(labelWidth, bar1Y, bar2Width, barH), 4))
            {
              using (SolidBrush b2Brush = new SolidBrush(ColorValue2))
              {
                g.FillPath(b2Brush, b2Path);
              }
            }
          }

          using (GraphicsPath b1Path = GetRoundedRect(new Rectangle(labelWidth, bar1Y, bar1Width, barH), 4))
          {
            using (SolidBrush b1Brush = new SolidBrush(ColorValue1))
            {
              g.FillPath(b1Brush, b1Path);
            }
          }

          // Value Badge Text
          string statText = item.Value2 > 0 ? $"{item.Value1}/{item.Value2}" : $"{item.Value1}";
          g.DrawString(statText, badgeFont, new SolidBrush(Color.FromArgb(100, 116, 139)), labelWidth + Math.Max(bar1Width, (int)(((double)item.Value2 / maxVal) * barMaxWidth)) + 8, curY + 4);
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