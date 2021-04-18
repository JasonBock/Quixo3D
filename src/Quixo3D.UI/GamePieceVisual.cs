using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Petzold.Media3D;
using Petzold.Text3D;
using Quixo3D.Framework;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace Quixo3D.UI
{
    /// <summary>
    /// The visual element used to represent a game piece on the board.
    /// </summary>
    public class GamePieceVisual : ModelVisual3D
    {
        const double BaseEmptyPieceOpacity = .8;
        const double BasePlacedPieceOpacity = .8;

        bool isValidSource;
        bool emptyModelShown;
        bool xModelShown;
        bool oModelShown;
        bool highlighted;
        bool highlightedAsValidSource;
        bool highlightedAsValidDestination;
        bool visible = true;
        bool enlarged;
        double lastEnlargeAmount;
        GeometryModel3D emptyModel;
        GeometryModel3D oModel;
        MaterialGroup emptyPieceMaterial;
        MaterialGroup placedPieceMaterial;
        PieceType pieceType = PieceType.Empty;
        ScaleTransform3D xAnimatedScale;
        ScaleTransform3D emptyAnimatedScale;
        ScaleTransform3D oAnimatedScale;
        SolidText xModel;
        TranslateTransform3D baseGridTransform;
        Transform3DGroup transforms;
        WirePath wireFrame;

        /// <summary>
        /// Creates a new instance of a <see cref="GamePieceVisual"/>.
        /// </summary>
        public GamePieceVisual()
        {
            emptyModel = this.CreateEmptyModel();
            xModel = this.CreateXModel();
            oModel = this.CreateOModel();
            wireFrame = GamePieceVisual.CreateCubeWireFrame();

            Model3DGroup group = new Model3DGroup();
            group.Children.Add(emptyModel);
            group.Children.Add(oModel);

            this.Content = group;
            this.Children.Add(wireFrame);
            this.Children.Add(xModel);

            this.transforms = new Transform3DGroup();
            this.baseGridTransform = new TranslateTransform3D();
            this.transforms.Children.Add(this.baseGridTransform);

            this.Transform = this.transforms;
        }

        /// <summary>
        /// The transform used to define the piece's location on the game board.
        /// </summary>
        public TranslateTransform3D BaseGridTransform
        {
            get { return this.baseGridTransform; }
            set { this.baseGridTransform = value; }
        }

        MaterialGroup EmptyPieceMaterial
        {
            get
            {
                if(this.emptyPieceMaterial == null)
                {
                    this.emptyPieceMaterial = new MaterialGroup();
                    SolidColorBrush diffuseBrush = new SolidColorBrush(Colors.White);
                    diffuseBrush.Opacity = BaseEmptyPieceOpacity;
                    DiffuseMaterial diffuse = new DiffuseMaterial(diffuseBrush);
                    this.emptyPieceMaterial.Children.Add(diffuse);
                }
                return this.emptyPieceMaterial;
            }
        }

        /// <summary>
        /// Gets the brush used to fill the material of the empty model.
        /// </summary>
        public SolidColorBrush EmptyPieceMaterialAnimatableBrush
        {
            get
            {
                DiffuseMaterial material = this.EmptyPieceMaterial.Children[0] as DiffuseMaterial;
                SolidColorBrush brush = material.Brush as SolidColorBrush;
                return brush;
            }
            set
            {
                DiffuseMaterial material = this.EmptyPieceMaterial.Children[0] as DiffuseMaterial;
                material.Brush = value;
            }
        }

        /// <summary>
        /// Gets the opacity of an empty game piece.
        /// </summary>
        public double EmptyPieceMaterialOpacity
        {
            get
            {
                return this.EmptyPieceMaterialAnimatableBrush.Opacity;
            }
        }

        /// <summary>
        /// Gets whether the piece has been enlarged.
        /// </summary>
        public bool IsEnlarged
        {
            get { return enlarged; }
        }

        /// <summary>
        /// Gets or sets whether the game piece is in a highlighted state.
        /// </summary>
        public bool Highlighted
        {
            get { return this.highlighted; }
            set
            {
                bool oldValue = this.highlighted;
                this.highlighted = value;
                if(oldValue != this.highlighted)
                {
                    this.OnHighlightedChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the game piece is selected as a valid destination piece.
        /// </summary>
        public bool HighlightedAsValidDestination
        {
            get
            {
                return this.highlightedAsValidDestination;
            }
            set
            {
                bool oldValue = highlightedAsValidDestination;
                this.highlightedAsValidDestination = value;
                if(oldValue != this.highlightedAsValidDestination)
                {
                    this.OnHighlightedAsValidDestinationChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the game piece is selected as a valid source piece.
        /// </summary>
        public bool HighlightedAsValidSource
        {
            get
            {
                return this.highlightedAsValidSource;
            }
            set
            {
                bool oldValue = this.highlightedAsValidSource;
                this.highlightedAsValidSource = value;
                if(oldValue != this.highlightedAsValidSource)
                {
                    this.OnHighlightedAsValidSourceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the game piece is a valid source and should
        /// clue the user in on whether or not it can be selected.
        /// </summary>
        public bool IsValidSource
        {
            get { return this.isValidSource; }
            set
            {
                this.isValidSource = value;
                this.OnIsValidSourceChanged();
            }
        }

        /// <summary>
        /// Gets the brush used to fill the mateiral of the O model.
        /// </summary>
        public SolidColorBrush OPieceMaterialAnimatableBrush
        {
            get
            {
                MaterialGroup materials = this.oModel.Material as MaterialGroup;
                DiffuseMaterial material = materials.Children[0] as DiffuseMaterial;
                SolidColorBrush brush = material.Brush as SolidColorBrush;
                return brush;
            }
        }

        /// <summary>
        /// Gets or sets whether the piece is an X, O, or Empty piece.
        /// </summary>
        public PieceType PieceType
        {
            get { return this.pieceType; }
            set
            {
                this.pieceType = value;
                this.OnPieceTypeChanged();
            }
        }

        MaterialGroup PlacedPieceMaterial
        {
            get
            {
                if(this.placedPieceMaterial == null)
                {
                    this.placedPieceMaterial = new MaterialGroup();
                    SolidColorBrush diffuseBrush = new SolidColorBrush(Colors.Red);
                    diffuseBrush.Opacity = BasePlacedPieceOpacity;
                    DiffuseMaterial diffuse = new DiffuseMaterial(diffuseBrush);
                    this.placedPieceMaterial.Children.Add(diffuse);
                }
                return this.placedPieceMaterial;
            }
        }
       
        /// <summary>
        /// Gets or sets whether the game piece is visible on the board.
        /// </summary>
        public bool Visible
        {
            get { return this.visible; }
            set
            {
                this.visible = value;
                this.OnVisibleChanged();
            }
        }

        /// <summary>
        /// Gets the brush used to fill the material of the X model.
        /// </summary>
        public SolidColorBrush XPieceMaterialAnimatableBrush
        {
            get
            {
                MaterialGroup materials = this.xModel.Material as MaterialGroup;
                DiffuseMaterial material = materials.Children[0] as DiffuseMaterial;
                SolidColorBrush brush = material.Brush as SolidColorBrush;
                return brush;
            }

        }

        static void AnimateHighlight(SolidColorBrush targetBrush, double start, double end)
        {
            if(targetBrush != null)
            {
                DoubleAnimation animation = new DoubleAnimation(start, end, TimeSpan.FromMilliseconds(50));
                targetBrush.BeginAnimation(SolidColorBrush.OpacityProperty, animation);
            }
        }

        static void AnimateScale(ScaleTransform3D targetScale, double start, double end)
        {
            if(targetScale != null)
            {
                DoubleAnimation animation = new DoubleAnimation(start, end, TimeSpan.FromMilliseconds(100));
                targetScale.BeginAnimation(ScaleTransform3D.ScaleXProperty, animation);
                targetScale.BeginAnimation(ScaleTransform3D.ScaleYProperty, animation);
                targetScale.BeginAnimation(ScaleTransform3D.ScaleZProperty, animation);
            }
        }

        static WirePath CreateCubeWireFrame()
        {
            WirePath wirePath = new WirePath();

            wirePath.LineCollection.Add(new Point3D(1, 1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, 1, 1));
            wirePath.LineCollection.Add(new Point3D(1, -1, 1));
            wirePath.LineCollection.Add(new Point3D(1, 1, 1));

            wirePath.LineCollection.Add(new Point3D(-1, -1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, 1, 1));
            wirePath.LineCollection.Add(new Point3D(1, -1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, -1, 1));

            wirePath.LineCollection.Add(new Point3D(1, 1, -1));
            wirePath.LineCollection.Add(new Point3D(-1, 1, -1));
            wirePath.LineCollection.Add(new Point3D(1, -1, -1));
            wirePath.LineCollection.Add(new Point3D(1, 1, -1));

            wirePath.LineCollection.Add(new Point3D(-1, -1, -1));
            wirePath.LineCollection.Add(new Point3D(-1, 1, -1));
            wirePath.LineCollection.Add(new Point3D(1, -1, -1));
            wirePath.LineCollection.Add(new Point3D(-1, -1, -1));

            wirePath.LineCollection.Add(new Point3D(1, 1, -1));
            wirePath.LineCollection.Add(new Point3D(1, 1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, 1, -1));
            wirePath.LineCollection.Add(new Point3D(1, 1, -1));

            wirePath.LineCollection.Add(new Point3D(-1, 1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, 1, -1));
            wirePath.LineCollection.Add(new Point3D(1, 1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, 1, 1));

            wirePath.LineCollection.Add(new Point3D(1, -1, -1));
            wirePath.LineCollection.Add(new Point3D(1, -1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, -1, -1));
            wirePath.LineCollection.Add(new Point3D(1, -1, -1));

            wirePath.LineCollection.Add(new Point3D(-1, -1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, -1, -1));
            wirePath.LineCollection.Add(new Point3D(1, -1, 1));
            wirePath.LineCollection.Add(new Point3D(-1, -1, 1));

            wirePath.Thickness = 0;
            return wirePath;

        }

        GeometryModel3D CreateEmptyModel()
        {
            CubeMesh cubeMesh = new CubeMesh();
            GeometryModel3D model = new GeometryModel3D();
            cubeMesh.Slices = 3;
            model.Geometry = cubeMesh.Geometry;

            Transform3DGroup transformGroup = new Transform3DGroup();
            ScaleTransform3D scale = new ScaleTransform3D(1, 1, 1);
            this.emptyAnimatedScale = new ScaleTransform3D(.9, .9, .9);

            transformGroup.Children.Add(scale);
            transformGroup.Children.Add(this.emptyAnimatedScale);

            model.Transform = transformGroup;

            return model;
        }

        GeometryModel3D CreateOModel()
        {
            TorusMesh mesh = new TorusMesh();
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = mesh.Geometry;            

            Transform3DGroup transformGroup = new Transform3DGroup();
            ScaleTransform3D scale = new ScaleTransform3D(.8, .8, .8);
            this.oAnimatedScale = new ScaleTransform3D(1, 1, 1);

            transformGroup.Children.Add(scale);
            transformGroup.Children.Add(this.oAnimatedScale);
            model.Transform = transformGroup;

            return model;
        }

        SolidText CreateXModel()
        {
            SolidText text = new SolidText();
            text.Text = "X";
            text.FontFamily = new FontFamily("Consolas");
            text.FontSize = 1;
            text.Depth = .1;
            text.SideMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Pink));
            Transform3DGroup transformGroup = new Transform3DGroup();
            ScaleTransform3D scale = new ScaleTransform3D(3, 3, 3);
            TranslateTransform3D translate = new TranslateTransform3D(-.9, 1.7, .1);
            this.xAnimatedScale = new ScaleTransform3D(1, 1, 1);

            transformGroup.Children.Add(scale);
            transformGroup.Children.Add(translate);
            transformGroup.Children.Add(this.xAnimatedScale);
            text.Transform = transformGroup;

            return text;
        }
        
        void Enlarge()
        {
            this.Enlarge(.2);
        }

        /// <summary>
        /// Makes the game piece bigger by a specified amount.  
        /// </summary>
        /// <param name="amount">A value of 1.00 will result in no change.  0.50 will result in a 50% 
        /// decrease in size.  2.00 will result in a 200% increase in size.</param>
        public void Enlarge(double amount)
        {

            double baseScale = .9;
            double endScale = baseScale + amount;
            this.lastEnlargeAmount = amount;

            if(emptyModelShown && pieceType == PieceType.Empty)
            {
                GamePieceVisual.AnimateScale(this.emptyAnimatedScale, baseScale, endScale);
                GamePieceVisual.AnimateHighlight(this.EmptyPieceMaterialAnimatableBrush, BaseEmptyPieceOpacity, BaseEmptyPieceOpacity + .1);
            }
            else if(xModelShown && pieceType == PieceType.X)
            {
                GamePieceVisual.AnimateScale(this.xAnimatedScale, baseScale, endScale);
                GamePieceVisual.AnimateHighlight(this.XPieceMaterialAnimatableBrush, BasePlacedPieceOpacity, 1);
            }
            else if(oModelShown && pieceType == PieceType.O)
            {
                GamePieceVisual.AnimateScale(this.oAnimatedScale, baseScale, endScale);
                GamePieceVisual.AnimateHighlight(this.OPieceMaterialAnimatableBrush, BasePlacedPieceOpacity, 1);
            }
            this.enlarged = true;
        }

        void HideEmptyModel()
        {
            this.emptyModel.Material = null;
            this.emptyModel.BackMaterial = null;
            this.emptyModelShown = false;
        }

        void HideOModel()
        {
            this.oModel.Material = null;
            this.oModel.BackMaterial = null;
            this.oModelShown = false;
        }

        /// <summary>
        /// Hides the thin, wire box that surrounds the piece.
        /// </summary>
        public void HideWireFrame()
        {
            this.wireFrame.Color = Colors.Transparent;
            this.wireFrame.Thickness = 0;
        }

        void HideXModel()
        {
            this.xModel.SideMaterial = null;
            this.xModel.Material = null;
            this.xModel.BackMaterial = null;
            this.xModelShown = false;
        }

        void OnHighlightedAsValidSourceChanged()
        {
            if(this.highlightedAsValidSource && this.emptyModelShown && this.pieceType == PieceType.Empty)
            {
                GamePieceVisual.AnimateHighlightAsValidSource(this.EmptyPieceMaterialAnimatableBrush, Colors.White, Colors.Yellow, GamePieceVisual.BaseEmptyPieceOpacity, 1);
            }
            else if(this.highlightedAsValidSource == false && this.emptyModelShown && this.pieceType == PieceType.Empty)
            {
                GamePieceVisual.AnimateHighlightAsValidSource(this.EmptyPieceMaterialAnimatableBrush, Colors.Yellow, Colors.White, 1, GamePieceVisual.BaseEmptyPieceOpacity);
                if(this.enlarged)
                {
                    this.ShrinkToNormalSize();
                }
            }
        }

        static void AnimateHighlightAsValidSource(SolidColorBrush targetBrush, Color startColor, Color endColor, double startOpacity, double endOpacity)
        {
            GamePieceVisual.AnimateColorAndOpacity(targetBrush, startColor, endColor, startOpacity, endOpacity);
        }

        static void AnimateHighlightAsValidDestination(SolidColorBrush targetBrush, Color startColor, Color endColor, double startOpacity, double endOpacity)
        {
            GamePieceVisual.AnimateColorAndOpacity(targetBrush, startColor, endColor, startOpacity, endOpacity);
        }

        static void AnimateColorAndOpacity(SolidColorBrush targetBrush, Color startColor, Color endColor, double startOpacity, double endOpacity)
        {
            ColorAnimation colorAnimation = new ColorAnimation(startColor, endColor, TimeSpan.FromMilliseconds(200));
            DoubleAnimation opacityAnimation = new DoubleAnimation(startOpacity, endOpacity, TimeSpan.FromMilliseconds(200));
            targetBrush.BeginAnimation(SolidColorBrush.OpacityProperty, opacityAnimation);
            targetBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        void OnHighlightedAsValidDestinationChanged()
        {
            if(this.highlightedAsValidDestination)
            {
                if(this.emptyModelShown && this.pieceType == PieceType.Empty)
                {
                    GamePieceVisual.AnimateHighlightAsValidDestination(this.EmptyPieceMaterialAnimatableBrush, Colors.White, Colors.LawnGreen, GamePieceVisual.BaseEmptyPieceOpacity, 1);
                }
                else if(this.xModelShown && this.pieceType == PieceType.X)
                {
                    GamePieceVisual.AnimateHighlightAsValidDestination(this.XPieceMaterialAnimatableBrush, Colors.Red, Colors.LawnGreen, GamePieceVisual.BasePlacedPieceOpacity, 1);
                }
                else if(this.oModelShown && this.pieceType == PieceType.O)
                {
                    GamePieceVisual.AnimateHighlightAsValidDestination(this.OPieceMaterialAnimatableBrush, Colors.Red, Colors.LawnGreen, GamePieceVisual.BasePlacedPieceOpacity, 1);
                }
            }
            else
            {
                if(this.emptyModelShown && this.pieceType == PieceType.Empty)
                {
                    GamePieceVisual.AnimateHighlightAsValidDestination(this.EmptyPieceMaterialAnimatableBrush, Colors.LawnGreen, Colors.White, 1, GamePieceVisual.BaseEmptyPieceOpacity);
                }
                else if(this.xModelShown && this.pieceType == PieceType.X)
                {
                    GamePieceVisual.AnimateHighlightAsValidDestination(this.XPieceMaterialAnimatableBrush, Colors.LawnGreen, Colors.Red, 1, GamePieceVisual.BasePlacedPieceOpacity);
                }
                else if(this.oModelShown && this.pieceType == PieceType.O)
                {
                    GamePieceVisual.AnimateHighlightAsValidDestination(this.OPieceMaterialAnimatableBrush, Colors.LawnGreen, Colors.Red, 1, GamePieceVisual.BasePlacedPieceOpacity);
                }
            }
        }

        /// <summary>
        /// Removes any highlighting and returns the piece back to a normal state.
        /// </summary>
        public void Reset()
        {
            this.Highlighted = false;
            this.HighlightedAsValidDestination = false;
            this.HighlightedAsValidSource = false;
            this.Visible = true;
        }
        
        void ShowEmptyModel()
        {
            this.emptyModel.Material = this.EmptyPieceMaterial;
            this.emptyModel.BackMaterial = this.EmptyPieceMaterial;
            this.emptyModelShown = true;
        }

        void ShowOModel()
        {
            this.oModel.Material = this.PlacedPieceMaterial;
            this.oModel.BackMaterial = this.PlacedPieceMaterial;
            this.oModelShown = true;
        }

        void ShowXModel()
        {
            this.xModel.SideMaterial = this.PlacedPieceMaterial;
            this.xModel.Material = this.PlacedPieceMaterial;
            this.xModel.BackMaterial = this.PlacedPieceMaterial;
            this.xModelShown = true;
        }

        /// <summary>
        /// Shows a thin, wire box around the piece.
        /// </summary>
        public void ShowWireFrame()
        {
            this.wireFrame.Color = Colors.Yellow;
            this.wireFrame.Thickness = .5;

            WireBase wirebase = wireFrame as WireBase;
            GeometryModel3D model = wirebase.Content as GeometryModel3D;
            MaterialGroup matgrp = model.Material as MaterialGroup;
            EmissiveMaterial mat = matgrp.Children[1] as EmissiveMaterial;
            SolidColorBrush brush = mat.Brush as SolidColorBrush;
            brush.Opacity = .7;

        }

        /// <summary>
        /// Sizes the game piece back to its original size.
        /// </summary>
        public void ShrinkToNormalSize()
        {
            double baseScale = .9;
            double endScale = baseScale + lastEnlargeAmount;

            if(emptyModelShown && pieceType == PieceType.Empty)
            {
                GamePieceVisual.AnimateScale(this.emptyAnimatedScale, endScale, baseScale);
                GamePieceVisual.AnimateHighlight(this.EmptyPieceMaterialAnimatableBrush, BaseEmptyPieceOpacity + .1, BaseEmptyPieceOpacity);
            }
            else if(xModelShown && pieceType == PieceType.X)
            {
                GamePieceVisual.AnimateScale(this.xAnimatedScale, endScale, baseScale);
                GamePieceVisual.AnimateHighlight(this.XPieceMaterialAnimatableBrush, 1, BasePlacedPieceOpacity);
            }
            else if(oModelShown && pieceType == PieceType.O)
            {
                GamePieceVisual.AnimateScale(this.oAnimatedScale, endScale, baseScale);
                GamePieceVisual.AnimateHighlight(this.OPieceMaterialAnimatableBrush, 1, BasePlacedPieceOpacity);
            }

            this.enlarged = false;
        }

        void OnVisibleChanged()
        {
            if(!this.Visible)
            {
                this.HideEmptyModel();
                this.HideXModel();
                this.HideOModel();
                this.HideWireFrame();
            }
            else
            {
                this.HideEmptyModel();
                this.HideXModel();
                this.HideOModel();

                if(this.IsValidSource)
                {
                    this.ShowWireFrame();
                }

                if(this.pieceType == PieceType.X)
                {
                    this.ShowXModel();
                }
                else if(this.pieceType == PieceType.O)
                {
                    this.ShowOModel();
                }
                else if(this.pieceType == PieceType.Empty && IsValidSource)
                {
                    this.ShowEmptyModel();
                }

            }
        }

        void OnHighlightedChanged()
        {
            if(this.HighlightedAsValidDestination || this.HighlightedAsValidSource)
            {
                return;
            }

            if(highlighted)
            {
                this.Enlarge();
            }
            else
            {
                this.ShrinkToNormalSize();
            }
        }

        void OnIsValidSourceChanged()
        {
            if(this.IsValidSource)
            {
                this.ShowWireFrame();

                if(pieceType == PieceType.Empty && emptyModelShown == false)
                {
                    this.ShowEmptyModel();
                }
                else
                {
                    this.HideEmptyModel();
                }
            }
            else
            {
                this.HideWireFrame();
                if(pieceType == PieceType.Empty)
                {
                    this.HideEmptyModel();
                }
            }
        }

        void OnPieceTypeChanged()
        {
            if(pieceType == PieceType.Empty)
            {
                this.HideXModel();
                this.HideOModel();

                if(IsValidSource)
                {
                    this.ShowEmptyModel();
                }
                else
                {
                    this.HideEmptyModel();
                }
            }
            else if(pieceType == PieceType.X)
            {
                this.HideEmptyModel();
                this.HideOModel();
                this.ShowXModel();
            }
            else if(pieceType == PieceType.O)
            {
                this.HideEmptyModel();
                this.HideXModel();
                this.ShowOModel();
            }
        }

    }
}
