namespace JMTControls.NetCore.Controls
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class ClosableToolStripMenuItem : ToolStripMenuItem
    {
        private const int CloseButtonWidth = 16;  // Tamaño del botón
        private const int CloseButtonMargin = 1;  // Margen derecho reducido
        private const int CloseButtonRightPadding = 4;  // Margen derecho reducido
        private const int CloseButtonPadding = 2; // Espacio interno de la X
        private Rectangle closeButtonRect;
        private bool isCloseButtonHovered = false;

        public event EventHandler CloseButtonClick;

        public ClosableToolStripMenuItem(string text) : base(text)
        {
            // Asegurar suficiente padding derecho
            this.Padding = new Padding(0, 0, 0, 0);
        }

        public static void ApplyRenderer(ToolStrip menuStrip)
        {
            menuStrip.Renderer = new ClosableMenuItemRenderer();
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool wasHovered = isCloseButtonHovered;
            isCloseButtonHovered = closeButtonRect.Contains(e.Location);
            if (wasHovered != isCloseButtonHovered)
                this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (isCloseButtonHovered)
            {
                isCloseButtonHovered = false;
                this.Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (closeButtonRect.Contains(e.Location))
            {
                OnCloseButtonClick(EventArgs.Empty);
                return;
            }
            base.OnMouseDown(e);
        }

        protected virtual void OnCloseButtonClick(EventArgs e)
        {
            CloseButtonClick?.Invoke(this, e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (!closeButtonRect.Contains(this.GetCurrentParent().PointToClient(Cursor.Position)))
            {
                base.OnClick(e);
                if (this.Tag is int index && this.GetCurrentParent()?.Parent is Form parentForm)
                {
                    if (index < parentForm.MdiChildren.Length)
                    {
                        parentForm.MdiChildren[index].BringToFront();
                    }
                }
            }
        }


        private class ClosableMenuItemRenderer : ToolStripProfessionalRenderer
        {

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected) // Selected = cuando hay hover
                {
                    // Color de fondo al hacer hover
                    using (var brush = new SolidBrush(Color.FromArgb(51, 153, 255))) // Azul
                    {
                        e.Graphics.FillRectangle(brush, new Rectangle(0, 0, e.Item.Width, e.Item.Height));
                    }
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                // Configurar el ancho del ítem
                e.Item.AutoSize = false;
                e.Item.Width = e.TextRectangle.Width + CloseButtonWidth + (CloseButtonMargin * 2) + 4;

                // Área de texto (deja espacio para el botón)
                var textRect = new Rectangle(
                    1,
                    e.TextRectangle.Y,
                    e.TextRectangle.Width,
                    e.TextRectangle.Height);

                // Cambiar color de texto cuando hay hover
                Color textColor = e.Item.Selected ? Color.White : e.TextColor;

                // Dibujar texto
                TextRenderer.DrawText(
                    e.Graphics,
                    e.Text,
                    e.TextFont,
                    textRect,
                    textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

                if (e.Item is ClosableToolStripMenuItem parentItem)
                {
                    // Calcular posición del botón de cierre
                    parentItem.closeButtonRect = new Rectangle(
                        e.Item.Width - CloseButtonWidth - CloseButtonRightPadding,
                        e.TextRectangle.Y + (e.TextRectangle.Height - CloseButtonWidth) / 2,
                        CloseButtonWidth,
                        CloseButtonWidth);

                    // Dibujar la X
                    using (var pen = new Pen(parentItem.isCloseButtonHovered ? Color.Red : Color.Gray, 2))
                    {
                        e.Graphics.DrawLine(pen,
                            parentItem.closeButtonRect.Left + CloseButtonPadding,
                            parentItem.closeButtonRect.Top + CloseButtonPadding,
                            parentItem.closeButtonRect.Right - CloseButtonPadding,
                            parentItem.closeButtonRect.Bottom - CloseButtonPadding);
                        e.Graphics.DrawLine(pen,
                            parentItem.closeButtonRect.Left + CloseButtonPadding,
                            parentItem.closeButtonRect.Bottom - CloseButtonPadding,
                            parentItem.closeButtonRect.Right - CloseButtonPadding,
                            parentItem.closeButtonRect.Top + CloseButtonPadding);
                    }

                    // Dibujar fondo hover del botón X
                    if (parentItem.isCloseButtonHovered)
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(30, Color.Red)))
                        {
                            e.Graphics.FillRectangle(brush, parentItem.closeButtonRect);
                        }
                    }
                }
            }

        }

    }

}