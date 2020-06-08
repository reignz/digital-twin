using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System;
using System.Threading;
using System.Globalization;
using System.Linq;

public class DBMySQLUtils : MonoBehaviour
{
    List<User> u;
    List<Requirement> r;
    List<Notification> n;
    List<Task> t;

    public static List<User> users;
    public static List<Requirement> requirements;
    public static List<Notification> notifications;
    public static List<Task> tasks;

    public static bool haveChangeUsers;
    public static bool haveChangeRequirements;
    public static bool haveChangeNotifications;
    public static bool haveChangeTasks;

    public static Thread thread0;
    public static Timer timer;

    static string constr = "Server=remotemysql.com;Database=7FHdKoJdhK;User ID=7FHdKoJdhK;Password=QQlitHnvoj;Port=3306;Pooling=true;CharSet=utf8;";
    // соединение 
    MySqlConnection con = new MySqlConnection(constr);
    // команда к БД
    MySqlCommand cmd = null;
    // чтение
    public static MySqlDataReader rdr = null;
    // ошибки
    MySqlError er = null;

    [SerializeField]
    private float notificationsReadingRate = 1f;

    [SerializeField]
    private float usersReadingRate = 1f;

    [SerializeField]
    private float requirementsReadingRate = 1f;

    protected virtual void Awake()
    {
        //try
        //{
        //    // установка элемента соединения 
        //    con = new MySqlConnection(constr);

        //    // посмотрим, сможем ли мы установить соединение 
        //    con.Open();
        //    Debug.Log("Connection State: " + con.State);
        //}
        //catch (IOException ex) { Debug.Log(ex.ToString()); }
    }

    protected virtual void OnApplicationQuit()
    {
        StopTimerAndThread();
        //Debug.Log("killing con");
        if (con != null)
        {
            if (con.State.ToString() != "Closed")
                con.Close();
            con.Dispose();
        }
    }

    void Start()
    {
        ReadAllInfoThread();
    }

    void Update()
    {

    }

    public void StopTimerAndThread()
    {
        timer.Change(Timeout.Infinite, Timeout.Infinite);
        thread0.Abort();
        //Debug.Log("Stop timer&threads");
    }

    public void ReadAllInfoThread()
    {
        var startTimeSpan = TimeSpan.Zero;
        var periodTimeSpan = TimeSpan.FromMinutes(1);

        timer = new Timer((e) =>
        {
            thread0 = new Thread(delegate ()
            {
                users = ReadUsers();
                requirements = ReadRequirement();
                notifications = ReadNotification();
                tasks = ReadTask();

                if (u != users)
                {
                    haveChangeUsers = true;
                }
                else
                {
                    haveChangeUsers = false;
                }

                if (r != requirements)
                {
                    haveChangeRequirements = true;
                }
                else
                {
                    haveChangeRequirements = false;
                }

                if (n != notifications)
                {
                    haveChangeNotifications = true;
                }
                else
                {
                    haveChangeNotifications = false;
                }

                if (t != tasks)
                {
                    haveChangeTasks = true;
                }
                else
                {
                    haveChangeTasks = false;
                }

                u = users;
                r = requirements;
                n = notifications;
                t = tasks;
            });
            thread0.Start();
            thread0.IsBackground = true;
        }, null, startTimeSpan, periodTimeSpan);
    }

    public void InsertUser(User user)
    {
        string query = string.Empty;
        int sex;

        if (user.Sex == true)
        {
            sex = 1;
        }
        else
        {
            sex = 0;
        }

        query = "INSERT INTO User (ID, Surname, Name, Patronymic, Birthday, Sex, Position, Status, Email, Password) VALUES (?ID, ?Surname, ?Name, ?Patronymic, ?Birthday, ?Sex, ?Position, ?Status, ?Email, ?Password)";

        if (con.State.ToString() != "Open")
            con.Open();
        using (con)
        {
            using (cmd = new MySqlCommand(query, con))
            {
                MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                oParam.Value = null;
                MySqlParameter oParam1 = cmd.Parameters.Add("?Surname", MySqlDbType.VarChar);
                oParam1.Value = user.Surname;
                MySqlParameter oParam2 = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                oParam2.Value = user.Name;
                MySqlParameter oParam3 = cmd.Parameters.Add("?Patronymic", MySqlDbType.VarChar);
                oParam3.Value = user.Patronymic;
                MySqlParameter oParam4 = cmd.Parameters.Add("?Birthday", MySqlDbType.DateTime);
                oParam4.Value = user.Birthday;
                MySqlParameter oParam5 = cmd.Parameters.Add("?Sex", MySqlDbType.Int32);
                oParam5.Value = sex;
                MySqlParameter oParam6 = cmd.Parameters.Add("?Position", MySqlDbType.VarChar);
                oParam6.Value = user.Position;
                MySqlParameter oParam7 = cmd.Parameters.Add("?Status", MySqlDbType.VarChar);
                oParam7.Value = user.Status;
                MySqlParameter oParam8 = cmd.Parameters.Add("?Email", MySqlDbType.VarChar);
                oParam8.Value = user.Email;
                MySqlParameter oParam9 = cmd.Parameters.Add("?Password", MySqlDbType.VarChar);
                oParam9.Value = user.Password;

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void UpdateUser(User user)
    {
        Debug.Log("update user");
        string query = string.Empty;
        int sex;

        if (user.Sex == true)
        {
            sex = 1;
        }
        else
        {
            sex = 0;
        }

        rdr.Close();

        query = "UPDATE User SET Surname=?Surname, Name=?Name, Patronymic=?Patronymic, Birthday=?Birthday, Sex=?Sex, Position=?Position, Status=?Status, Email=?Email, Password=?Password WHERE ID=?ID";

        if (con.State.ToString() != "Open")
            con.Open();
        using (con)
        {
            using (cmd = new MySqlCommand(query, con))
            {
                MySqlParameter oParam1 = cmd.Parameters.Add("?Surname", MySqlDbType.VarChar);
                oParam1.Value = user.Surname;
                MySqlParameter oParam2 = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                oParam2.Value = user.Name;
                MySqlParameter oParam3 = cmd.Parameters.Add("?Patronymic", MySqlDbType.VarChar);
                oParam3.Value = user.Patronymic;
                MySqlParameter oParam4 = cmd.Parameters.Add("?Birthday", MySqlDbType.DateTime);
                oParam4.Value = user.Birthday;
                MySqlParameter oParam5 = cmd.Parameters.Add("?Sex", MySqlDbType.Int32);
                oParam5.Value = sex;
                MySqlParameter oParam6 = cmd.Parameters.Add("?Position", MySqlDbType.VarChar);
                oParam6.Value = user.Position;
                MySqlParameter oParam7 = cmd.Parameters.Add("?Status", MySqlDbType.VarChar);
                oParam7.Value = user.Status;
                MySqlParameter oParam8 = cmd.Parameters.Add("?Email", MySqlDbType.VarChar);
                oParam8.Value = user.Email;
                MySqlParameter oParam9 = cmd.Parameters.Add("?Password", MySqlDbType.VarChar);
                oParam9.Value = user.Password;
                MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                oParam.Value = user.ID;
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void DeleteUser(User user)
    {
        string query = string.Empty;

        try
        {
            query = "DELETE FROM User WHERE ID=?ID";

            if (con.State.ToString() != "Open")
                con.Open();
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = user.ID;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally { con.Close(); }
    }

    public List<User> ReadUsers()
    {
        List<User> usersList = new List<User>();
        string query = string.Empty;

        query = "SELECT ID, Surname, Name, Patronymic, Birthday, Sex, Position, Status, Email, Password FROM User";

        if (con.State.ToString() != "Open")
            con.Open();

        cmd = new MySqlCommand(query, con);

        if (rdr == null || rdr.IsClosed)
        {
            rdr = cmd.ExecuteReader();
        }

        while (rdr.Read())
        {
            User user = new User();

            user.ID = int.Parse(rdr[0].ToString());
            user.Surname = rdr[1].ToString();
            user.Name = rdr[2].ToString();
            user.Patronymic = rdr[3].ToString();
            user.Birthday = DateTime.Parse(rdr[4].ToString());

            if (int.Parse(rdr[5].ToString()) == 0)
            { user.Sex = false; }
            else
            { user.Sex = true; }

            user.Position = rdr[6].ToString();
            user.Status = rdr[7].ToString();
            user.Email = rdr[8].ToString();
            user.Password = rdr[9].ToString();

            usersList.Add(user);
        }

        ProcessUsers(usersList);

        con.Close();
        return usersList;
    }

    public User ReadUserByID(int ID)
    {
        List<User> usersList = new List<User>();
        string query = string.Empty;

        query = "SELECT ID, Surname, Name, Patronymic, Birthday, Sex, Position, Status, Email, Password FROM User WHERE ID=" + ID;

        if (con.State.ToString() != "Open")
            con.Open();

        cmd = new MySqlCommand(query, con);

        if (rdr == null || rdr.IsClosed)
        {
            rdr = cmd.ExecuteReader();
        }

        while (rdr.Read())
        {
            User user = new User();

            user.ID = int.Parse(rdr[0].ToString());
            user.Surname = rdr[1].ToString();
            user.Name = rdr[2].ToString();
            user.Patronymic = rdr[3].ToString();
            user.Birthday = DateTime.Parse(rdr[4].ToString());

            if (int.Parse(rdr[5].ToString()) == 0)
            { user.Sex = false; }
            else
            { user.Sex = true; }

            user.Position = rdr[6].ToString();
            user.Status = rdr[7].ToString();
            user.Email = rdr[8].ToString();
            user.Password = rdr[9].ToString();

            usersList.Add(user);
        }

        ProcessUsers(usersList);

        User u = usersList.First();
        return u;
    }

    protected virtual void ProcessUsers(List<User> usersList)
    {
        rdr.Close();
    }

    public void InsertRequirement(Requirement requirement)
    {
        string query = string.Empty;

        try
        {
            query = "INSERT INTO Requirement (ID, Name, SerialNumber, Lifetime, ImplementationDate, Status) VALUES (?ID, ?Name, ?SerialNumber, ?Lifetime, ?ImplementationDate, ?Status)";

            if (con.State.ToString() != "Open")
                con.Open();
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = null;
                    MySqlParameter oParam1 = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam1.Value = requirement.Name;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?SerialNumber", MySqlDbType.VarChar);
                    oParam2.Value = requirement.SerialNumber;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?Lifetime", MySqlDbType.Int32);
                    oParam3.Value = requirement.Lifetime;
                    MySqlParameter oParam4 = cmd.Parameters.Add("?ImplementationDate", MySqlDbType.DateTime);
                    oParam4.Value = requirement.ImplementationDate;
                    MySqlParameter oParam5 = cmd.Parameters.Add("?Status", MySqlDbType.VarChar);
                    oParam5.Value = requirement.Status;

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally { con.Close(); }
    }

    public bool InsertRequirementWithParameters(Requirement requirement)
    {
        string query = string.Empty;

        try
        {
            query = "INSERT INTO Requirement (ID, Name, SerialNumber, Lifetime, ImplementationDate, Status, " +
                "PressureParameter1, PressureParameter2, PressureParameter3, PressureParameter4," +
                "TemperatureParameter1, TemperatureParameter2, TemperatureParameter3, TemperatureParameter4" +
                ") VALUES (?ID, ?Name, ?SerialNumber, ?Lifetime, ?ImplementationDate, ?Status, " +
                "?PressureParameter1, ?PressureParameter2, ?PressureParameter3, ?PressureParameter4, " +
                "?TemperatureParameter1, ?TemperatureParameter2, ?TemperatureParameter3, ?TemperatureParameter4)";

            if (con.State.ToString() != "Open")
                con.Open();
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = null;
                    MySqlParameter oParam1 = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam1.Value = requirement.Name;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?SerialNumber", MySqlDbType.VarChar);
                    oParam2.Value = requirement.SerialNumber;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?Lifetime", MySqlDbType.Int32);
                    oParam3.Value = requirement.Lifetime;
                    MySqlParameter oParam4 = cmd.Parameters.Add("?ImplementationDate", MySqlDbType.DateTime);
                    oParam4.Value = requirement.ImplementationDate;
                    MySqlParameter oParam5 = cmd.Parameters.Add("?Status", MySqlDbType.VarChar);
                    oParam5.Value = requirement.Status;

                    MySqlParameter oParam6 = cmd.Parameters.Add("?PressureParameter1", MySqlDbType.Int32);
                    oParam6.Value = requirement.PressureParameter1;
                    MySqlParameter oParam7 = cmd.Parameters.Add("?PressureParameter2", MySqlDbType.Int32);
                    oParam7.Value = requirement.PressureParameter2;
                    MySqlParameter oParam8 = cmd.Parameters.Add("?PressureParameter3", MySqlDbType.Int32);
                    oParam8.Value = requirement.PressureParameter3;
                    MySqlParameter oParam9 = cmd.Parameters.Add("?PressureParameter4", MySqlDbType.Int32);
                    oParam9.Value = requirement.PressureParameter4;

                    MySqlParameter oParam10 = cmd.Parameters.Add("?TemperatureParameter1", MySqlDbType.Int32);
                    oParam10.Value = requirement.TemperatureParameter1;
                    MySqlParameter oParam11 = cmd.Parameters.Add("?TemperatureParameter2", MySqlDbType.Int32);
                    oParam11.Value = requirement.TemperatureParameter2;
                    MySqlParameter oParam12 = cmd.Parameters.Add("?TemperatureParameter3", MySqlDbType.Int32);
                    oParam12.Value = requirement.TemperatureParameter3;
                    MySqlParameter oParam13 = cmd.Parameters.Add("?TemperatureParameter4", MySqlDbType.Int32);
                    oParam13.Value = requirement.TemperatureParameter4;

                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return false;
        }
        finally { con.Close(); }
    }

    public void UpdateRequirement(Requirement requirement)
    {
        Debug.Log("update requirement");
        string query = string.Empty;

        rdr.Close();

        query = "UPDATE Requirement SET Name=?Name, SerialNumber=?SerialNumber, Lifetime=?Lifetime, ImplementationDate=?ImplementationDate, Status=?Status WHERE ID=?ID";

        if (con.State.ToString() != "Open")
            con.Open();
        using (con)
        {
            using (cmd = new MySqlCommand(query, con))
            {
                MySqlParameter oParam1 = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                oParam1.Value = requirement.Name;
                MySqlParameter oParam2 = cmd.Parameters.Add("?SerialNumber", MySqlDbType.VarChar);
                oParam2.Value = requirement.SerialNumber;
                MySqlParameter oParam3 = cmd.Parameters.Add("?Lifetime", MySqlDbType.Int32);
                oParam3.Value = requirement.Lifetime;
                MySqlParameter oParam4 = cmd.Parameters.Add("?ImplementationDate", MySqlDbType.DateTime);
                oParam4.Value = requirement.ImplementationDate;
                MySqlParameter oParam5 = cmd.Parameters.Add("?Status", MySqlDbType.VarChar);
                oParam5.Value = requirement.Status;
                MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                oParam.Value = requirement.ID;

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void DeleteRequirement(Requirement requirement)
    {
        string query = string.Empty;

        try
        {
            query = "DELETE FROM Requirement WHERE ID=?ID";

            if (con.State.ToString() != "Open")
                con.Open();
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = requirement.ID;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally { con.Close(); }
    }

    public List<Requirement> ReadRequirement()
    {
        List<Requirement> requirementsList = new List<Requirement>();
        string query = string.Empty;

        query = "SELECT ID, Name, SerialNumber, Lifetime, ImplementationDate, Status FROM Requirement";

        if (con.State.ToString() != "Open")
            con.Open();

        cmd = new MySqlCommand(query, con);

        if (rdr == null || rdr.IsClosed)
        {
            rdr = cmd.ExecuteReader();
        }

        while (rdr.Read())
        {
            Requirement requirement = new Requirement();

            requirement.ID = int.Parse(rdr[0].ToString());
            requirement.Name = rdr[1].ToString();
            requirement.SerialNumber = rdr[2].ToString();
            requirement.Lifetime = int.Parse(rdr[3].ToString());
            requirement.ImplementationDate = DateTime.Parse(rdr[4].ToString());
            requirement.Status = rdr[5].ToString();
            requirementsList.Add(requirement);
        }

        ProcessRequirements(requirementsList);
        con.Close();
        return requirementsList;
    }

    //public Requirement ReadRequirementById(int reqId)
    //{

    //    Debug.Log("in ReadRequirementById()");
    //    List<Requirement> requirementsList = new List<Requirement>();
    //    string query = string.Empty;

    //    query = "SELECT ID, Name, Lifetime, ImplementationDate, Status FROM Requirement WHERE ID=" + reqId;

    //    if (con.State.ToString() != "Open")
    //        con.Open();

    //    cmd = new MySqlCommand(query, con);

    //    if (rdr == null || rdr.IsClosed)
    //    {
    //        rdr = cmd.ExecuteReader();
    //    }

    //    while (rdr.Read())
    //    {
    //        Requirement requirement = new Requirement();

    //        requirement.ID = int.Parse(rdr[0].ToString());
    //        requirement.Name = rdr[1].ToString();
    //        requirement.Lifetime = int.Parse(rdr[2].ToString());
    //        requirement.ImplementationDate = DateTime.Parse(rdr[3].ToString());
    //        requirement.Status = rdr[4].ToString();
    //        requirementsList.Add(requirement);
    //    }

    //    ProcessRequirements(requirementsList);

    //    Requirement r = requirementsList.First();
    //    return r;
    //}

    protected virtual void ProcessRequirements(List<Requirement> requirementsList)
    {
        rdr.Close();
    }

    public void InsertNotificationAboutUser(Notification notification, int UserID)
    {
        Debug.Log("InsertNotificationAboutUser");
        string query = string.Empty;

        int priority;
        if (notification.Priority == true)
        { priority = 1; }
        else { priority = 0; }

        int active;
        if (notification.Active == true)
        { active = 1; }
        else { active = 0; }

        rdr.Close();

        query = "INSERT INTO Notification (ID, Priority, Placeholder, Text, CreationDate, Requirement_ID, User_ID, Active) VALUES (?ID, ?Priority, ?Placeholder, ?Text, ?CreationDate, ?Requirement_ID, ?User_ID, ?Active)";

        if (con.State.ToString() != "Open")
            con.Open();
        using (con)
        {
            using (cmd = new MySqlCommand(query, con))
            {
                MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                oParam.Value = null;
                MySqlParameter oParam2 = cmd.Parameters.Add("?Priority", MySqlDbType.Int32);
                oParam2.Value = priority;
                MySqlParameter oParam3 = cmd.Parameters.Add("?Placeholder", MySqlDbType.VarChar);
                oParam3.Value = notification.Placeholder;
                MySqlParameter oParam4 = cmd.Parameters.Add("?Text", MySqlDbType.VarChar);
                oParam4.Value = notification.Text;
                MySqlParameter oParam5 = cmd.Parameters.Add("?CreationDate", MySqlDbType.DateTime);
                oParam5.Value = notification.CreationDate;
                MySqlParameter oParam6 = cmd.Parameters.Add("?Requirement_ID", MySqlDbType.Int32);
                oParam6.Value = null;
                MySqlParameter oParam7 = cmd.Parameters.Add("?User_ID", MySqlDbType.Int32);
                oParam7.Value = UserID;
                MySqlParameter oParam8 = cmd.Parameters.Add("Active", MySqlDbType.Int32);
                oParam8.Value = active;

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
    
    public void InsertNotificationAboutRequirement(Notification notification, int RequirementID)
    {
        //Debug.Log("InsertNotificationAboutRequirement");
        string query = string.Empty;

        int priority;
        if (notification.Priority == true)
        { priority = 1; }
        else { priority = 0; }

        int active;
        if (notification.Active == true)
        { active = 1; }
        else { active = 0; }

        rdr.Close();

        query = "INSERT INTO Notification (ID, Priority, Placeholder, Text, CreationDate, Requirement_ID, User_ID, Active) VALUES (?ID, ?Priority, ?Placeholder, ?Text, ?CreationDate, ?Requirement_ID, ?User_ID, ?Active)";

        if (con.State.ToString() != "Open")
            con.Open();
        using (con)
        {
            using (cmd = new MySqlCommand(query, con))
            {
                MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                oParam.Value = null;
                MySqlParameter oParam2 = cmd.Parameters.Add("?Priority", MySqlDbType.Int32);
                oParam2.Value = priority;
                MySqlParameter oParam3 = cmd.Parameters.Add("?Placeholder", MySqlDbType.VarChar);
                oParam3.Value = notification.Placeholder;
                MySqlParameter oParam4 = cmd.Parameters.Add("?Text", MySqlDbType.VarChar);
                oParam4.Value = notification.Text;
                MySqlParameter oParam5 = cmd.Parameters.Add("?CreationDate", MySqlDbType.DateTime);
                oParam5.Value = notification.CreationDate;
                MySqlParameter oParam6 = cmd.Parameters.Add("?Requirement_ID", MySqlDbType.Int32);
                oParam6.Value = RequirementID;
                MySqlParameter oParam7 = cmd.Parameters.Add("?User_ID", MySqlDbType.Int32);
                oParam7.Value = null;
                MySqlParameter oParam8 = cmd.Parameters.Add("Active", MySqlDbType.Int32);
                oParam8.Value = active;

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void UpdateNotification(Notification notification)
    {
        //Debug.Log("UpdateNotification");
        string query = string.Empty;

        int priority;
        if (notification.Priority == true)
        { priority = 1; }
        else { priority = 0; }

        int active;
        if (notification.Active == true)
        { active = 1; }
        else { active = 0; }

        if (notification.User_ID == 0) notification.User_ID = null;
        if (notification.Requirement_ID == 0) notification.Requirement_ID = null;

        rdr.Close();

        query = "UPDATE Notification SET Priority=?Priority, Placeholder=?Placeholder, Text=?Text, CreationDate=?CreationDate, Requirement_ID=?Requirement_ID, User_ID=?User_ID, Active=?Active WHERE ID=?ID";

        if (con.State.ToString() != "Open")
            con.Open();
        using (con)
        {
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                MySqlParameter oParam2 = cmd.Parameters.Add("?Priority", MySqlDbType.Int32);
                oParam2.Value = priority;
                MySqlParameter oParam3 = cmd.Parameters.Add("?Placeholder", MySqlDbType.VarChar);
                oParam3.Value = notification.Placeholder;
                MySqlParameter oParam4 = cmd.Parameters.Add("?Text", MySqlDbType.VarChar);
                oParam4.Value = notification.Text;
                MySqlParameter oParam5 = cmd.Parameters.Add("?CreationDate", MySqlDbType.DateTime);
                oParam5.Value = notification.CreationDate;
                MySqlParameter oParam6 = cmd.Parameters.Add("?Requirement_ID", MySqlDbType.Int32);
                oParam6.Value = notification.Requirement_ID;
                MySqlParameter oParam7 = cmd.Parameters.Add("?User_ID", MySqlDbType.Int32);
                oParam7.Value = notification.User_ID;
                MySqlParameter oParam8 = cmd.Parameters.Add("Active", MySqlDbType.Int32);
                oParam8.Value = active;
                MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                oParam.Value = notification.ID;

                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void DeleteNotification(Notification notification)
    {
        string query = string.Empty;

        try
        {
            query = "DELETE FROM Notification WHERE ID=?ID";

            if (con.State.ToString() != "Open")
                con.Open();
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = notification.ID;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally { con.Close(); }
    }

    public List<Notification> ReadNotification()
    {
        List<Notification> notificationsList = new List<Notification>();
        string query = string.Empty;

        query = "SELECT ID, Priority, Placeholder, Text, CreationDate, COALESCE(Requirement_ID, 0), COALESCE(User_ID, 0), Active FROM Notification";

        if (con.State.ToString() != "Open")
            con.Open();

        cmd = new MySqlCommand(query, con);

        if (rdr == null || rdr.IsClosed)
        {
            rdr = cmd.ExecuteReader();
        }

        while (rdr.Read())
        {
            Notification notification = new Notification();
            bool priority;
            bool activity;

            notification.ID = int.Parse(rdr[0].ToString());

            if (int.Parse(rdr[1].ToString()) == 1) priority = true;
            else priority = false;
            notification.Priority = priority;

            notification.Placeholder = rdr[2].ToString();
            notification.Text = rdr[3].ToString();
            notification.CreationDate = DateTime.Parse(rdr[4].ToString());

            if (int.Parse(rdr[5].ToString()) == 0) notification.Requirement_ID = null;
            else notification.Requirement_ID = int.Parse(rdr[5].ToString());

            if (int.Parse(rdr[6].ToString()) == 0) notification.User_ID = null;
            notification.User_ID = int.Parse(rdr[6].ToString());

            if (int.Parse(rdr[7].ToString()) == 1) activity = true;
            else activity = false;
            notification.Active = activity;

            notificationsList.Add(notification);
        }

        if (notificationsList.Count != 0)
        {
            ProcessNotifications(notificationsList);
        }
        else
        {
            ProcessNotifications();
        }
        con.Close();
        return notificationsList;
    }

    //public IEnumerator ReadActiveNotificationsByUserId(int userID)
    //{
    //    while (true)
    //    {
    //        List<Notification> notificationsList = new List<Notification>();
    //        string query = string.Empty;

    //        query = "SELECT ID, Priority, Placeholder, Text, CreationDate, COALESCE(Requirement_ID, 0), COALESCE(User_ID, 0), Active FROM Notification WHERE User_ID=" + userID + " AND Active=1";

    //        if (con.State.ToString() != "Open")
    //            con.Open();

    //        cmd = new MySqlCommand(query, con);

    //        if (rdr == null || rdr.IsClosed)
    //        {
    //            rdr = cmd.ExecuteReader();
    //        }

    //        while (rdr.Read())
    //        {
    //            Notification notification = new Notification();
    //            bool priority;
    //            bool activity;

    //            notification.ID = int.Parse(rdr[0].ToString());

    //            if (int.Parse(rdr[1].ToString()) == 1) priority = true;
    //            else priority = false;
    //            notification.Priority = priority;

    //            notification.Placeholder = rdr[2].ToString();
    //            notification.Text = rdr[3].ToString();
    //            notification.CreationDate = DateTime.Parse(rdr[4].ToString());

    //            if (int.Parse(rdr[5].ToString()) == 0) notification.Requirement_ID = null;
    //            else notification.Requirement_ID = int.Parse(rdr[5].ToString());

    //            if (int.Parse(rdr[6].ToString()) == 0) notification.User_ID = null;
    //            notification.User_ID = int.Parse(rdr[6].ToString());

    //            if (int.Parse(rdr[7].ToString()) == 1) activity = true;
    //            else activity = false;
    //            notification.Active = activity;

    //            notificationsList.Add(notification);
    //        }

    //        ProcessNotifications(notificationsList);

    //        yield return new WaitForSeconds(notificationsReadingRate);
    //    }
    //}

    //public IEnumerator ReadActiveNotificationsByReqId(int reqID)
    //{
    //    while (true)
    //    {
    //        List<Notification> notificationsList = new List<Notification>();
    //        string query = string.Empty;

    //        query = "SELECT ID, Priority, Placeholder, Text, CreationDate, COALESCE(Requirement_ID, 0), COALESCE(User_ID, 0), Active FROM Notification WHERE Requirement_ID=" + reqID + " AND Active=1";

    //        if (con.State.ToString() != "Open")
    //            con.Open();

    //        cmd = new MySqlCommand(query, con);

    //        if (rdr == null || rdr.IsClosed)
    //        {
    //            rdr = cmd.ExecuteReader();
    //        }

    //        while (rdr.Read())
    //        {
    //            Notification notification = new Notification();
    //            bool priority;
    //            bool activity;

    //            notification.ID = int.Parse(rdr[0].ToString());

    //            if (int.Parse(rdr[1].ToString()) == 1) priority = true;
    //            else priority = false;
    //            notification.Priority = priority;

    //            notification.Placeholder = rdr[2].ToString();
    //            notification.Text = rdr[3].ToString();
    //            notification.CreationDate = DateTime.Parse(rdr[4].ToString());

    //            if (int.Parse(rdr[5].ToString()) == 0) notification.Requirement_ID = null;
    //            else notification.Requirement_ID = int.Parse(rdr[5].ToString());

    //            if (int.Parse(rdr[6].ToString()) == 0) notification.User_ID = null;
    //            notification.User_ID = int.Parse(rdr[6].ToString());

    //            if (int.Parse(rdr[7].ToString()) == 1) activity = true;
    //            else activity = false;
    //            notification.Active = activity;

    //            notificationsList.Add(notification);
    //        }

    //        ProcessNotifications(notificationsList);

    //        yield return new WaitForSeconds(notificationsReadingRate);
    //    }
    //}

    protected virtual void ProcessNotifications(List<Notification> notificationsList)
    {
        rdr.Close();
    }

    protected virtual void ProcessNotifications()
    {
        rdr.Close();
    }

    public void InsertTask(Task task, User user, Requirement requirement)
    {
        string query = string.Empty;

        try
        {
            query = "INSERT INTO Task (ID, Name, Description, Date, Status, User_ID, Requirement_ID) VALUES (?ID, ?Name, ?Description, ?Date, ?Status, ?User_ID, ?Requirement_ID)";

            if (con.State.ToString() != "Open")
                con.Open();

            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = null;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam2.Value = task.Name;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?Description", MySqlDbType.VarChar);
                    oParam3.Value = task.Description;
                    MySqlParameter oParam4 = cmd.Parameters.Add("?Date", MySqlDbType.DateTime);
                    oParam4.Value = task.Date;
                    MySqlParameter oParam5 = cmd.Parameters.Add("?Status", MySqlDbType.VarChar);
                    oParam5.Value = task.Status;
                    MySqlParameter oParam6 = cmd.Parameters.Add("?User_ID", MySqlDbType.Int32);
                    oParam6.Value = user.ID;
                    MySqlParameter oParam7 = cmd.Parameters.Add("?Requirement_ID", MySqlDbType.Int32);
                    oParam7.Value = requirement.ID;

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally { con.Close(); }
    }

    public void UpdateTask(Task task)
    {
        string query = string.Empty;

        try
        {
            query = "UPDATE Task SET Name=?Name, Description=?Description, Date=?Date, Status=?Status, User_ID=?User_ID, Requirement_ID=?Requirement_ID WHERE ID=?ID";

            if (con.State.ToString() != "Open")
                con.Open();
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam2 = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam2.Value = task.Name;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?Description", MySqlDbType.VarChar);
                    oParam3.Value = task.Description;
                    MySqlParameter oParam4 = cmd.Parameters.Add("?Date", MySqlDbType.DateTime);
                    oParam4.Value = task.Date;
                    MySqlParameter oParam5 = cmd.Parameters.Add("?Status", MySqlDbType.VarChar);
                    oParam5.Value = task.Status;
                    MySqlParameter oParam6 = cmd.Parameters.Add("?User_ID", MySqlDbType.Int32);
                    oParam6.Value = task.User_ID;
                    MySqlParameter oParam7 = cmd.Parameters.Add("?Requirement_ID", MySqlDbType.Int32);
                    oParam7.Value = task.Requirement_ID;
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = task.ID;

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally { con.Close(); }
    }

    public void DeleteTask(Task task)
    {
        string query = string.Empty;

        try
        {
            query = "DELETE FROM Task WHERE ID=?ID";

            if (con.State.ToString() != "Open")
                con.Open();
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = task.ID;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        finally { con.Close(); }
    }

    public List<Task> ReadTask()
    {
        string query = string.Empty;
        List<Task> tasksList = new List<Task>();

        query = "SELECT ID, Name, Description, Date, Status, User_ID, Requirement_ID FROM Task";

        if (con.State.ToString() != "Open")
            con.Open();

        cmd = new MySqlCommand(query, con);

        if (rdr == null || rdr.IsClosed)
        {
            rdr = cmd.ExecuteReader();
        }

        while (rdr.Read())
        {
            Task task = new Task();

            task.ID = int.Parse(rdr[0].ToString());
            task.Name = rdr[1].ToString();
            task.Description = rdr[2].ToString();
            task.Date = DateTime.Parse(rdr[3].ToString());
            task.Status = rdr[4].ToString();
            task.User_ID = int.Parse(rdr[5].ToString());
            task.Requirement_ID = int.Parse(rdr[6].ToString());

            tasksList.Add(task);
        }

        ProcessTasks(tasksList);

        con.Close();
        return tasksList;
    }

    protected virtual void ProcessTasks(List<Task> tasksList)
    {
        rdr.Close();
    }
}
