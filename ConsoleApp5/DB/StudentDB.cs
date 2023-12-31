﻿using System.Collections.Generic;
using System.Text.Json;

class StudentDB
{
    Dictionary<string, Student> students;

    public StudentDB()
    {
        //load file (json)
        if (File.Exists("students.json"))
            using (var fs = File.OpenRead("students.json"))
                students = JsonSerializer.Deserialize<Dictionary<string, Student>>(fs);
        else
            students = new Dictionary<string, Student>();
    }

    public List<Student> Search(string text)
    {
        List<Student> result = new();
        foreach (var student in students.Values)
        {
            if (student.FirstName.Contains(text) ||
                    student.LastName.Contains(text))
                result.Add(student);
        }
        return result;
    }

    public bool Update(Student student)
    {
        if (!students.ContainsKey(student.UID))
            return false;
        students[student.UID] = student;
        Save();
        return true;
    }

    public Student Create()
    {
        Student newStudent = new Student {  UID = Guid.NewGuid().ToString()};
        students.Add(newStudent.UID, newStudent);
        return newStudent;
    }

    public bool Delete(Student student)
    {
        if (!students.ContainsKey(student.UID))
            return false;
        students.Remove(student.UID);
        Save();
        return true;
    }

    void Save()
    {
        // save file (json)
        using (var fs = File.Create("students.json"))
            JsonSerializer.Serialize(fs, students);
    }
}
