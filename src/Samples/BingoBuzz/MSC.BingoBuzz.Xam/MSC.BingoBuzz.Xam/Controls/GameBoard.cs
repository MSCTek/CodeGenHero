using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CodeGenHero.BingoBuzz.Xam.Controls
{
    public class GameBoard : ContentView, IDisposable
    {
        public static readonly BindableProperty GameBoardContentProperty = BindableProperty.Create(propertyName: "GameBoardContent",
              returnType: typeof(List<ModelObj.BB.BingoInstanceContent>),
              declaringType: typeof(GameBoard),
              propertyChanged: OnPropertyChanged,
              defaultValue: default(List<ModelObj.BB.BingoInstanceContent>));

        private bool _disposed;

        public GameBoard()
        {
            Init();
        }

        public List<ModelObj.BB.BingoInstanceContent> GameBoardContent
        {
            get { return (List<ModelObj.BB.BingoInstanceContent>)GetValue(GameBoardContentProperty); }
            set { SetValue(GameBoardContentProperty, value); }
        }

        public void CleanUp()
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
                CleanUp();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var ch = (GameBoard)bindable;
            ch.Init();
        }

        private void Init()
        {
            if (GameBoardContent != null && GameBoardContent.Any())
            {
                Grid _gameGrid = new Grid();

                foreach (var sq in GameBoardContent)
                {
                    Grid sqGrid = new Grid();
                    if (sq.IsSelected)
                    {
                        //selected
                        sqGrid.BackgroundColor = Color.Tomato;
                    }
                    else
                    {
                        //unselected
                        if (sq.FreeSquareIndicator)
                        {
                            sqGrid.BackgroundColor = Color.LightGray;
                        }
                        else
                        {
                            sqGrid.BackgroundColor = Color.AliceBlue;
                        }
                    }

                    DataTrigger trigger = new DataTrigger(typeof(Grid));
                    trigger.Binding = new Binding("IsSelected", BindingMode.Default, source: sq);
                    trigger.Value = true;
                    trigger.Setters.Add(new Setter { Property = Grid.BackgroundColorProperty, Value = Color.Aqua });
                    sqGrid.Triggers.Add(trigger);

                    TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
                    _tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "SquareTappedCommand", BindingMode.TwoWay);
                    _tapGestureRecognizer.CommandParameter = sq.BingoInstanceContentId;
                    sqGrid.GestureRecognizers.Add(_tapGestureRecognizer);

                    Label _label = new Label();
                    _label.Text = sq.BingoContent.Content;
                    _label.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    _label.VerticalOptions = LayoutOptions.CenterAndExpand;
                    _label.LineBreakMode = LineBreakMode.WordWrap;
                    _label.HorizontalTextAlignment = TextAlignment.Center;
                    _label.Margin = 3;
                    _label.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));

                    sqGrid.Children.Add(_label);
                    _gameGrid.Children.Add(sqGrid, sq.Col, sq.Row);
                }

                Content = _gameGrid;
            }
        }
    }
}