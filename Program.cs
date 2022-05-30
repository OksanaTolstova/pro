using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace FrontEnd
{
    class Flight  
    {
        public List<double> X = new List<double>() { 0 }; //начальное положение Х
        public List<double> Y = new List<double>() { 0 }; //начальное положение У
        public List<double> T = new List<double>() { 0 }; //начальное положение Т
        public List<double> Vx = new List<double>(); //координаты скорости V
        public List<double> Vy = new List<double>(); //координаты скорости V
        public Flight(double V, double a)
        {
            a = a * Math.PI / 180; //перевод значения угла в градусы
            const double g = 9.806; //значение своюодного падения
            const double m = 10; //масса 
            double k = Math.Sin(a); //закон сопротивления 

            Vx.Add(V * Math.Cos(a)); //новые координаты Х
            Vy.Add(V * Math.Sin(a)); //новые координаты У
            double dT = 0.05; //интервал времени, между подсчетом координатццццццццццццццццццц
            int i = 0; //цикл 
            while (Y[i] >= 0) 
            {                                  //формулы вычисления значений координат 
                X.Add(X[i] + dT * Vx[i]);
                Y.Add(Y[i] + dT * Vy[i]);
                T.Add(T[i] + dT);
                Vx.Add(Vx[i] - dT * k * Vx[i] / m);
                Vy.Add(Vy[i] - dT * (g + k * Vy[i] / m));
                i++;
            }
        }
    }

    class Enter : Window     
         
    {

        [STAThread]
        public static void Main()
        {
            Application app = new Application(); 
            app.Run(new Enter()); 
        }
        public Enter()
        {

            Title = "Полёт"; //название заголовка окна

            ResizeMode = ResizeMode.CanMinimize;  //пользовать может свернуть окно и восстановить с панели задач,отображается только свернуть
            StackPanel stackMain = new StackPanel(); 
            stackMain.Orientation = Orientation.Horizontal;  //выравнивает дочерние элементы в одну линию по горизонтали
            stackMain.Margin = new Thickness(5); //определяет толщины рамки вокруг прямоугольника
            Content = stackMain; 

            StackPanel stackChild1 = new StackPanel();  
            stackMain.Children.Add(stackChild1); 

            Label lbl1 = new Label(); //класс,представляет собой текстовую надпись для элемента и поддерживает использование клавиш
            lbl1.Content = "Введите скорость тела: "; //надпись первого элемента с указание ввода скорости
            lbl1.Background = Brushes.Gray; //цвет элемента
            lbl1.Margin = new Thickness(); //значение поля, по умолчанию 0
            stackChild1.Children.Add(lbl1); 
            Label lbl2 = new Label(); //класс,представляет собой текстовую надпись для элемента и поддерживает использование клавиш
            lbl2.Content = "Введите угол: "; //надпись второго элемента с указанием ввести угол
            lbl2.Background = Brushes.YellowGreen; //цвет элемента
            lbl2.Margin = new Thickness(10); //значение поля (поле - пространство между элементом и и элеметнами смежными с ним)
            stackChild1.Children.Add(lbl2); 

            StackPanel stackChild2 = new StackPanel(); 
            stackMain.Children.Add(stackChild2); 
             
            TextBox txtbox1 = new TextBox(); //класс, позволяющий отобржать и редактировать текст
            txtbox1.Margin = new Thickness(10); //значение поля
            txtbox1.Width = 64; //ширина элемента
            stackChild2.Children.Add(txtbox1); 
            TextBox txtbox2 = new TextBox(); //класс, позволяющий отобржать и редактировать текст
            txtbox2.Margin = new Thickness(10); //значение поля
            stackChild2.Children.Add(txtbox2); 

            ScrollViewer sv = new ScrollViewer(); //класс, создает бегунок для прокрутки
            stackMain.Children.Add(sv); 

            Canvas canv = new Canvas(); //область, в которой размешается дочерний элемент с помощью коррдинат
            canv.Width = 200; //ширина элемента
            canv.Height = 100; //высота элемента
            stackMain.Children.Add(canv);

            Button btn = new Button(); //класс кнопка
            btn.Background = Brushes.Pink; //цвет кнопки
            btn.VerticalAlignment = VerticalAlignment.Center; //расположение
            btn.Name = "Рассчитать"; //имя кнопки
            btn.Content = btn.Name; //отображение имени
            btn.Margin = new Thickness(1); //значение поля
            btn.Padding = new Thickness(10); //пространство между дочерним элемнтом и границей или фоном
            btn.Click += ButtonOnClick; //событие, происходящие при нажатии 
            stackChild2.Children.Add(btn); 
        
            

            void ButtonOnClick(object sender, RoutedEventArgs args)
            {
                Flight b = new Flight(Convert.ToDouble(txtbox1.Text), Convert.ToDouble(txtbox2.Text)); //передача данных из TextBox 
                 
                //вывод таблицы
                string enter = "X\tY\tT\n";  
                for (int n = 0; n < b.T.Count; n++) //цикл от 0 до n меньше b, шагом плюс 1
                {
                    enter += b.X[n] + "\t" + b.Y[n] + "\t" + b.T[n] + "\n"; 
                }
                MessageBox.Show(b.X[b.X.Count - 1] + " " + b.Y[b.Y.Count - 1], "Тело упало"); //вывод сообщения на экран
                sv.Content = enter;
                 
                //рисует траекторию
                Polyline poly = new Polyline();  //класс рисует последовательность соединенных прямых линий
                canv.Children.Add(poly); 
                Point[] pts = new Point[b.T.Count]; //описывает точки вершин
                for (int i = 0; i < b.T.Count; i++) //цикл, передающий координаты 
                {
                    pts[i].X = b.X[i];
                    pts[i].Y = -b.Y[i] + 100;
                }
                poly.Points = new PointCollection(pts); //коллекция значений пар координат в двумерном пространстве
                poly.Stroke = Brushes.Black; //рисование контура черным цветом
                poly.StrokeThickness = 4; //ширина контура

                {
                    DoubleAnimation aB = new DoubleAnimation(); //класс анимации 
                    aB.Duration = new Duration(TimeSpan.FromSeconds(30)); //время которое длится анимация
                    aB.From = 5; //значение до начала анимации
                    aB.To = 17; //значение которое достигает при анимации
                    aB.FillBehavior = FillBehavior.Stop;  //после анимации возвращает к первоначальному виду
                    btn.BeginAnimation(Button.FontSizeProperty, aB); //класс, где происходит анимация 
                }

            }
        }
    }
}
