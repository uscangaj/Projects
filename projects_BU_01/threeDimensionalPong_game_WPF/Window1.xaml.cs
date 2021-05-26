//Juan Uscanga, CS 480, Homework 08
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Security.Cryptography;
using systemio = System.IO;
using System.Media;



namespace uscanga_CS480_HW08
{
    public partial class Window1 : Window
    {
        //Time
        DispatcherTimer timer;                          //timer
        private double inc = 0.0;                       //timer increment counter
        private double amt = 0.25;                      //distance increment

        //Camera
        private PerspectiveCamera TheCamera;
        private double CameraPhi = Math.PI / 7.0;       // vertical degrees
        private double CameraTheta = Math.PI / 3.5;     // horizontal degrees
        private double CameraR = 27.5;                  // radius distance
        private const double CameraDPhi = 0.1;         //vertical camera change (up , down)
        private const double CameraDTheta = 0.1;       //horizontal camera change (left , right)
        private const double CameraDR = 0.3;            //camera zoom change (+ , -)

        //Objects
        private Model3DGroup MainModel3Dgroup = new Model3DGroup(); //object model group

        //Sphere
        DiffuseMaterial materialSphere;                          //sphere mesh material
        GeometryModel3D modelSphere;                             //sphere model
        MeshGeometry3D meshSphere;                               //mesh for sphere
        Point3D sp = new Point3D(1, 9, 1);                  //start center point for sphere
        Point3D np = new Point3D(1, 9, 1);                  //new updated center point for sphere
        int anglep = 10;                                    //angle phi
        int anglet = 20;                                    //angle theta
        double srad = 0.5;                                  //sphere radius
        double ox = 3.0;                                    //sphere original x position
        double oy = 5.0;                                    //sphere original y position
        double oz = 7.0;                                    //sphere original z position
        double sx;                                          //sphere x position
        double sy;                                          //sphere y position
        double sz;                                          //sphere z position

        //Shadow
        DiffuseMaterial materialShadow;                          //sphere mesh material
        GeometryModel3D modelShadow;                             //sphere model
        MeshGeometry3D meshShadow;                               //mesh for sphere
        Point3D shadowsp = new Point3D(1, 9, 1);                     //start center point for sphere
        Point3D shadownp = new Point3D(1, 9, 1);                     //new updated center point for sphere
        double shadowrad = 0.5;                                     //sphere radius
        double shadowy = 7.0;                                    //shadow y position
        Boolean vert;                                            //flag for sphere vertical travel

        //Movement
        double vx = 0.4;
        double vy = 0.5;
        double vz = 0.6;

        double xlow = 0.35;
        double xhigh = 9.4;
        double ylow = 0.6;
        double yhigh = 9.4;
        double zlow = 0.6;
        double zhigh = 9.35;

        Random rng = new Random();

        //Graphics
        SolidColorBrush brushSphere = Brushes.LightGray; //sphere
        SolidColorBrush brushShadow = Brushes.DarkGray; //shadow
        SolidColorBrush brushR = Brushes.Red;           //X
        SolidColorBrush brushG = Brushes.Green;         //Y
        SolidColorBrush brushB = Brushes.Blue;          //Z

        //Sound
        SoundPlayer soundbounce;
        String bouncefile;
        String bouncepath;



        public Window1()
        {
            InitializeComponent();

            //Sound path
            String bouncefile = "jump.wav";
            bouncepath = systemio.Path.Combine(Environment.CurrentDirectory, @"Sounds\");
            bouncepath = systemio.Path.Combine(bouncepath, bouncefile);
            soundbounce = new SoundPlayer(bouncepath);
        }



        //Load Scene
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //initalize camera position
            TheCamera = new PerspectiveCamera();
            TheCamera.FieldOfView = 75;
            MainViewport.Camera = TheCamera;
            PositionCamera();

            //Initialize light source
            DefineLights();

            //Create object model
            DefineModel(MainModel3Dgroup);

            //Add model
            ModelVisual3D model_visual = new ModelVisual3D();
            model_visual.Content = MainModel3Dgroup;

            //Disply viewport
            MainViewport.Children.Add(model_visual);

            //Initialize Timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += UpdateTimer;
            timer.Start();

            //Initialize sphere center point
            sx = ox;
            sy = oy;
            sz = oz;
            sp = new Point3D(sx, sy, sz);
            np = sp;

            vert = true;
        }



        //Update timer
        private void UpdateTimer(object sender, EventArgs e)
        {
            //Remove previous sphere object
            MainModel3Dgroup.Children.Remove(modelSphere);
            MainModel3Dgroup.Children.Remove(modelShadow);

            //Update sphere center point
            sx = sx + vx;
            sy = sy + vy;
            sz = sz + vz;

            //Check boundaries
            //X
            if (sx > xhigh)
            {
                soundbounce.Play();

                vx *= -1;
            }
            else if (sx < xlow)
            {
                soundbounce.Play();

                vx *= -1;
            }
            //Y
            if (sy > yhigh)
            {
                soundbounce.Play();

                vy *= -1;
                vert = false;
            }
            else if (sy < ylow)
            {
                soundbounce.Play();

                vy *= -1;
                vert = true;
            }
            //Z
            if (sz > zhigh)
            {
                soundbounce.Play();

                vz *= -1;
            }
            else if (sz < zlow)
            {
                soundbounce.Play();

                vz *= -1;
            }

            //Update shadow
            if (vert==true && shadowrad>0.3) //sphere going up
            {
                shadowrad -= 0.025;
            }
            else if (vert==false && shadowrad<0.5) //sphere going down
            {
                shadowrad += 0.025;
            }

            //Create new sphere object
            np = new Point3D(sx, sy, sz);
            meshSphere = new MeshGeometry3D();
            AddSmoothSphere(meshSphere, np, srad, anglep, anglet);
            materialSphere = new DiffuseMaterial(brushSphere);
            modelSphere = new GeometryModel3D(meshSphere, materialSphere);
            MainModel3Dgroup.Children.Add(modelSphere);

            //Create shadow on floor
            //private void AddSmoothCylinder(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius, int num_sides)
            meshShadow = new MeshGeometry3D();
            AddSmoothCylinder(meshShadow, new Point3D(sx, -0.01, sz),
                new Vector3D(0, 0.01, 0), shadowrad, 10);
            materialShadow = new DiffuseMaterial(brushShadow);
            modelShadow = new GeometryModel3D(meshShadow, materialShadow);
            MainModel3Dgroup.Children.Add(modelShadow);


            //Update timer
            inc += 0.5;
            label_text.Content = "X,Y,Z: " + sx.ToString() + ", " + sy.ToString() + ", " + sz.ToString();
        }



        //Define light source
        private void DefineLights()
        {
            AmbientLight ambient_light = new AmbientLight(Colors.Gray);
            DirectionalLight directional_light =
                new DirectionalLight(Colors.Gray, new Vector3D(2.0, 8.0, 2.0));
            MainModel3Dgroup.Children.Add(ambient_light);
            MainModel3Dgroup.Children.Add(directional_light);
        }



        //Add models to group
        private void DefineModel(Model3DGroup model_group)
        {
            //private void AddSmoothCylinder(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius, int num_sides)
            /* BOTTOM WALLS */
            //Make bottom-back cylinder along the X axis
            MeshGeometry3D meshX1 = new MeshGeometry3D();
            AddSmoothCylinder(meshX1, new Point3D(0, 0, 0),
                new Vector3D(0, 0, 15), 0.05, 10);
            DiffuseMaterial materialX1 = new DiffuseMaterial(brushR);
            GeometryModel3D modelX1 = new GeometryModel3D(meshX1, materialX1);
            model_group.Children.Add(modelX1);

            //Make bottom-front cylinder along the X axis.
            MeshGeometry3D meshX2 = new MeshGeometry3D();
            AddSmoothCylinder(meshX2, new Point3D(10, 0, 0),
                new Vector3D(0, 0, 10), 0.05, 10);
            DiffuseMaterial materialX2 = new DiffuseMaterial(brushR);
            GeometryModel3D modelX2 = new GeometryModel3D(meshX2, materialX2);
            model_group.Children.Add(modelX2);

            //Make "bottom-right" cylinder along the Y axis
            MeshGeometry3D meshZ1 = new MeshGeometry3D();
            AddSmoothCylinder(meshZ1, new Point3D(0, 0, 0),
                new Vector3D(15, 0, 0), 0.05, 10);
            DiffuseMaterial materialZ1 = new DiffuseMaterial(brushG);
            GeometryModel3D modelZ1 = new GeometryModel3D(meshZ1, materialZ1);
            model_group.Children.Add(modelZ1);
            
            //Make "bottom-left" cylinder along the Y end axis
            MeshGeometry3D meshZ2 = new MeshGeometry3D();
            AddSmoothCylinder(meshZ2, new Point3D(0, 0, 10),
                new Vector3D(10, 0, 0), 0.05, 10);
            DiffuseMaterial materialZ2 = new DiffuseMaterial(brushG);
            GeometryModel3D modelZ2 = new GeometryModel3D(meshZ2, materialZ2);
            model_group.Children.Add(modelZ2);


            /* VERTICAL WALLS */
            //Make back-right cylinder along the Z axis
            MeshGeometry3D meshY1 = new MeshGeometry3D();
            AddSmoothCylinder(meshY1, new Point3D(0, 0, 0),
                new Vector3D(0, 15, 0), 0.05, 10);
            DiffuseMaterial materialY1 = new DiffuseMaterial(brushB);
            GeometryModel3D modelY1 = new GeometryModel3D(meshY1, materialY1);
            model_group.Children.Add(modelY1);

            //Make back-left cylinder along the Z axis
            MeshGeometry3D meshY2 = new MeshGeometry3D();
            AddSmoothCylinder(meshY2, new Point3D(0, 0, 10),
                new Vector3D(0, 10, 0), 0.05, 10);
            DiffuseMaterial materialY2 = new DiffuseMaterial(brushB);
            GeometryModel3D modelY2 = new GeometryModel3D(meshY2, materialY2);
            model_group.Children.Add(modelY2);

            //Make front-right cylinder along the Z axis
            MeshGeometry3D meshZ3 = new MeshGeometry3D();
            AddSmoothCylinder(meshZ3, new Point3D(10, 0, 0),
                new Vector3D(0, 10, 0), 0.05, 10);
            DiffuseMaterial materialZ3 = new DiffuseMaterial(brushB);
            GeometryModel3D modelZ3 = new GeometryModel3D(meshZ3, materialZ3);
            model_group.Children.Add(modelZ3);
            
            //Make front-left cylinder along the Z end axis
            MeshGeometry3D meshZ4 = new MeshGeometry3D();
            AddSmoothCylinder(meshZ4, new Point3D(10, 0, 10),
                new Vector3D(0, 10, 0), 0.05, 10);
            DiffuseMaterial materialZ4 = new DiffuseMaterial(brushB);
            GeometryModel3D modelZ4 = new GeometryModel3D(meshZ4, materialZ4);
            model_group.Children.Add(modelZ4);



            /* TOP WALLS */
            // Make top-back cylinder along the X axis.
            MeshGeometry3D meshX3 = new MeshGeometry3D();
            AddSmoothCylinder(meshX3, new Point3D(0, 10, 0),
                new Vector3D(0, 0, 10), 0.05, 10);
            DiffuseMaterial materialX3 = new DiffuseMaterial(brushR);
            GeometryModel3D modelX3 = new GeometryModel3D(meshX3, materialX3);
            model_group.Children.Add(modelX3);

            // Make top-front cylinder along the X axis.
            MeshGeometry3D meshX4 = new MeshGeometry3D();
            AddSmoothCylinder(meshX4, new Point3D(10, 10, 0),
                new Vector3D(0, 0, 10), 0.05, 10);
            DiffuseMaterial materialX4 = new DiffuseMaterial(brushR);
            GeometryModel3D modelX4 = new GeometryModel3D(meshX4, materialX4);
            model_group.Children.Add(modelX4);

            // Make top-right cylinder along the Y axis.
            MeshGeometry3D meshY3 = new MeshGeometry3D();
            AddSmoothCylinder(meshY3, new Point3D(0, 10, 0),
                new Vector3D(10, 0, 0), 0.05, 10);
            DiffuseMaterial materialY3 = new DiffuseMaterial(brushG);
            GeometryModel3D modelY3 = new GeometryModel3D(meshY3, materialY3);
            model_group.Children.Add(modelY3);

            // Make top-left cylinder along the Y axis.
            MeshGeometry3D meshY4 = new MeshGeometry3D();
            AddSmoothCylinder(meshY4, new Point3D(0, 10, 10),
                new Vector3D(10, 0, 0), 0.05, 10);
            DiffuseMaterial materialY4 = new DiffuseMaterial(brushG);
            GeometryModel3D modelY4 = new GeometryModel3D(meshY4, materialY4);
            model_group.Children.Add(modelY4);
        }
        


        //Add a triangle to the indicated mesh (Do not reuse points so triangles don't share normals)
        private void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            //Create the points
            int index1 = mesh.Positions.Count;
            mesh.Positions.Add(point1);
            mesh.Positions.Add(point2);
            mesh.Positions.Add(point3);

            //Create the triangle
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1);
        }



        //Add a triangle to the indicated mesh (Reuse points so triangles share normals)
        private void AddSmoothTriangle(MeshGeometry3D mesh, Dictionary<Point3D, int> dict, Point3D point1, Point3D point2, Point3D point3)
        {
            int index1, index2, index3;

            //Find or create the points
            if (dict.ContainsKey(point1)) index1 = dict[point1];
            else
            {
                index1 = mesh.Positions.Count;
                mesh.Positions.Add(point1);
                dict.Add(point1, index1);
            }

            if (dict.ContainsKey(point2)) index2 = dict[point2];
            else
            {
                index2 = mesh.Positions.Count;
                mesh.Positions.Add(point2);
                dict.Add(point2, index2);
            }

            if (dict.ContainsKey(point3)) index3 = dict[point3];
            else
            {
                index3 = mesh.Positions.Count;
                mesh.Positions.Add(point3);
                dict.Add(point3, index3);
            }

            //If two or more of the points are the same, then its not a triangle
            if ((index1 == index2) ||
                (index2 == index3) ||
                (index3 == index1)) return;

            //Create the triangle
            mesh.TriangleIndices.Add(index1);
            mesh.TriangleIndices.Add(index2);
            mesh.TriangleIndices.Add(index3);
        }



        //Make a thin rectangular prism between the two points
        private void AddSegment(MeshGeometry3D mesh,
            Point3D point1, Point3D point2, Vector3D up)
        {
            AddSegment(mesh, point1, point2, up, false);
        }
        private void AddSegment(MeshGeometry3D mesh,
            Point3D point1, Point3D point2, Vector3D up,
            bool extend)
        {
            const double thickness = 0.25;

            //Get segment vector
            Vector3D v = point2 - point1;

            if (extend)
            {
                //Increase segment length on both ends by thickness / 2
                Vector3D n = ScaleVector(v, thickness / 2.0);
                point1 -= n;
                point2 += n;
            }

            //Get the scaled up vector
            Vector3D n1 = ScaleVector(up, thickness / 2.0);

            //Get another scaled perpendicular vector
            Vector3D n2 = Vector3D.CrossProduct(v, n1);
            n2 = ScaleVector(n2, thickness / 2.0);

            //Make a skinny box
            Point3D p1pp = point1 + n1 + n2;
            Point3D p1mp = point1 - n1 + n2;
            Point3D p1pm = point1 + n1 - n2;
            Point3D p1mm = point1 - n1 - n2;
            Point3D p2pp = point2 + n1 + n2;
            Point3D p2mp = point2 - n1 + n2;
            Point3D p2pm = point2 + n1 - n2;
            Point3D p2mm = point2 - n1 - n2;

            //Make sides
            AddTriangle(mesh, p1pp, p1mp, p2mp);
            AddTriangle(mesh, p1pp, p2mp, p2pp);

            AddTriangle(mesh, p1pp, p2pp, p2pm);
            AddTriangle(mesh, p1pp, p2pm, p1pm);

            AddTriangle(mesh, p1pm, p2pm, p2mm);
            AddTriangle(mesh, p1pm, p2mm, p1mm);

            AddTriangle(mesh, p1mm, p2mm, p2mp);
            AddTriangle(mesh, p1mm, p2mp, p1mp);

            //Make Ends
            AddTriangle(mesh, p1pp, p1pm, p1mm);
            AddTriangle(mesh, p1pp, p1mm, p1mp);

            AddTriangle(mesh, p2pp, p2mp, p2mm);
            AddTriangle(mesh, p2pp, p2mm, p2pm);
        }



        //Add a cage
        private void AddCage(MeshGeometry3D mesh)
        {
            //Create top
            Vector3D up = new Vector3D(0, 1, 0);
            AddSegment(mesh, new Point3D(1, 1, 1), new Point3D(1, 1, -1), up, true);
            AddSegment(mesh, new Point3D(1, 1, -1), new Point3D(-1, 1, -1), up, true);
            AddSegment(mesh, new Point3D(-1, 1, -1), new Point3D(-1, 1, 1), up, true);
            AddSegment(mesh, new Point3D(-1, 1, 1), new Point3D(1, 1, 1), up, true);

            //Create bottom
            AddSegment(mesh, new Point3D(1, -1, 1), new Point3D(1, -1, -1), up, true);
            AddSegment(mesh, new Point3D(1, -1, -1), new Point3D(-1, -1, -1), up, true);
            AddSegment(mesh, new Point3D(-1, -1, -1), new Point3D(-1, -1, 1), up, true);
            AddSegment(mesh, new Point3D(-1, -1, 1), new Point3D(1, -1, 1), up, true);

            //Create sides
            Vector3D right = new Vector3D(1, 0, 0);
            AddSegment(mesh, new Point3D(1, -1, 1), new Point3D(1, 1, 1), right);
            AddSegment(mesh, new Point3D(1, -1, -1), new Point3D(1, 1, -1), right);
            AddSegment(mesh, new Point3D(-1, -1, 1), new Point3D(-1, 1, 1), right);
            AddSegment(mesh, new Point3D(-1, -1, -1), new Point3D(-1, 1, -1), right);
        }



        //Set a vector length
        private Vector3D ScaleVector(Vector3D vector, double length)
        {
            double scale = length / vector.Length;
            return new Vector3D(
                vector.X * scale,
                vector.Y * scale,
                vector.Z * scale);
        }



        //Update camera position
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    CameraPhi += CameraDPhi;
                    if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
                    break;
                case Key.Down:
                    CameraPhi -= CameraDPhi;
                    if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
                    break;
                case Key.Left:
                    CameraTheta += CameraDTheta;
                    break;
                case Key.Right:
                    CameraTheta -= CameraDTheta;
                    break;
                case Key.Add:
                case Key.OemPlus:
                    CameraR -= CameraDR;
                    if (CameraR < CameraDR) CameraR = CameraDR;
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    CameraR += CameraDR;
                    break;
            }

            //Update camera position
            PositionCamera();
        }



        //Position camera
        private void PositionCamera()
        {
            //Calculate camera position (using Cartesian coordinates)
            double y = CameraR * Math.Sin(CameraPhi);
            double hyp = CameraR * Math.Cos(CameraPhi);
            double x = hyp * Math.Cos(CameraTheta);
            double z = hyp * Math.Sin(CameraTheta);
            TheCamera.Position = new Point3D(x, y, z);

            //Point at origin
            TheCamera.LookDirection = new Vector3D(-x, -y, -z);

            //Set Up direction
            TheCamera.UpDirection = new Vector3D(0, 1, 0);
        }



        //Add a cylinder with smooth sides
        private void AddSmoothCylinder(MeshGeometry3D mesh, Point3D end_point, Vector3D axis, double radius, int num_sides)
        {
            //Get two vectors perpendicular to the axis
            Vector3D v1;
            if ((axis.Z < -0.01) || (axis.Z > 0.01))
                v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
            else
                v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
            Vector3D v2 = Vector3D.CrossProduct(v1, axis);

            //Make the vectors have length radius
            v1 *= (radius / v1.Length);
            v2 *= (radius / v2.Length);

            // Make the top cap, bottom cap, and end point
            int pt0 = mesh.Positions.Count; //Index of end_point
            mesh.Positions.Add(end_point);

            //Make the top points
            double theta = 0;
            double dtheta = 2 * Math.PI / num_sides;
            for (int i = 0; i < num_sides; i++)
            {
                mesh.Positions.Add(end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2);
                theta += dtheta;
            }

            //ake the top triangles
            int pt1 = mesh.Positions.Count - 1; //Index of last point
            int pt2 = pt0 + 1;                  //Index of first point in this cap
            for (int i = 0; i < num_sides; i++)
            {
                mesh.TriangleIndices.Add(pt0);
                mesh.TriangleIndices.Add(pt1);
                mesh.TriangleIndices.Add(pt2);
                pt1 = pt2++;
            }

            //Make the bottom end cap and end point.
            pt0 = mesh.Positions.Count;         //Index of end_point2
            Point3D end_point2 = end_point + axis;
            mesh.Positions.Add(end_point2);

            //Make the bottom points
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                mesh.Positions.Add(end_point2 +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2);
                theta += dtheta;
            }

            //Make the bottom triangles
            theta = 0;
            pt1 = mesh.Positions.Count - 1;     //Index of last point.
            pt2 = pt0 + 1;                      //Index of first point in this cap.
            for (int i = 0; i < num_sides; i++)
            {
                mesh.TriangleIndices.Add(num_sides + 1);    //end_point2
                mesh.TriangleIndices.Add(pt2);
                mesh.TriangleIndices.Add(pt1);
                pt1 = pt2++;
            }

            //Make the sides
            //Add the points to the mesh
            int first_side_point = mesh.Positions.Count;
            theta = 0;
            for (int i = 0; i < num_sides; i++)
            {
                Point3D p1 = end_point +
                    Math.Cos(theta) * v1 +
                    Math.Sin(theta) * v2;
                mesh.Positions.Add(p1);
                Point3D p2 = p1 + axis;
                mesh.Positions.Add(p2);
                theta += dtheta;
            }

            //Make the side triangles
            pt1 = mesh.Positions.Count - 2;
            pt2 = pt1 + 1;
            int pt3 = first_side_point;
            int pt4 = pt3 + 1;
            for (int i = 0; i < num_sides; i++)
            {
                mesh.TriangleIndices.Add(pt1);
                mesh.TriangleIndices.Add(pt2);
                mesh.TriangleIndices.Add(pt4);

                mesh.TriangleIndices.Add(pt1);
                mesh.TriangleIndices.Add(pt4);
                mesh.TriangleIndices.Add(pt3);

                pt1 = pt3;
                pt3 += 2;
                pt2 = pt4;
                pt4 += 2;
            }
        }



        //Add a sphere
        private void AddSmoothSphere(MeshGeometry3D mesh, Point3D center, double radius, int num_phi, int num_theta)
        {
            //Make a dictionary to track the sphere's points
            Dictionary<Point3D, int> dict = new Dictionary<Point3D, int>();

            double phi0, theta0;
            double dphi = Math.PI / num_phi;
            double dtheta = 2 * Math.PI / num_theta;

            phi0 = 0;
            double y0 = radius * Math.Cos(phi0);
            double r0 = radius * Math.Sin(phi0);
            for (int i = 0; i < num_phi; i++)
            {
                double phi1 = phi0 + dphi;
                double y1 = radius * Math.Cos(phi1);
                double r1 = radius * Math.Sin(phi1);


                //Find points where theta = theta0
                theta0 = 0;
                Point3D pt00 = new Point3D(
                    center.X + r0 * Math.Cos(theta0),
                    center.Y + y0,
                    center.Z + r0 * Math.Sin(theta0));
                Point3D pt10 = new Point3D(
                    center.X + r1 * Math.Cos(theta0),
                    center.Y + y1,
                    center.Z + r1 * Math.Sin(theta0));
                for (int j = 0; j < num_theta; j++)
                {
                    //Find the points where theta = theta1
                    double theta1 = theta0 + dtheta;
                    Point3D pt01 = new Point3D(
                        center.X + r0 * Math.Cos(theta1),
                        center.Y + y0,
                        center.Z + r0 * Math.Sin(theta1));
                    Point3D pt11 = new Point3D(
                        center.X + r1 * Math.Cos(theta1),
                        center.Y + y1,
                        center.Z + r1 * Math.Sin(theta1));

                    //Create the triangles
                    AddSmoothTriangle(mesh, dict, pt00, pt11, pt10);
                    AddSmoothTriangle(mesh, dict, pt00, pt01, pt11);

                    //Move to the next value of theta
                    theta0 = theta1;
                    pt00 = pt01;
                    pt10 = pt11;
                }

                //Move to the next phi value
                phi0 = phi1;
                y0 = y1;
                r0 = r1;
            }
        }
    }
}
