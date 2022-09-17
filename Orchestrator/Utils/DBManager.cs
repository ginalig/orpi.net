using Environment = Orchestrator.Models.Environment;

namespace Orchestrator.Utils;

public class DbManager
{
    private static List<Environment> db = new List<Environment>();

    public void Insert(Environment environment)
    {
        db.Add(environment);
    }

    public void Update(Environment environment)
    {
        if (db.Count == 0)
        {
            throw new ApplicationException("List is empty!");
        }
        for (var i = 0; i < db.Count; i++)
        {
            var item = db[i];
            if (environment.ID == item.ID)
            {
                db[i] = environment;
            }
        }
    }

    public void Delete(Environment environment)
    {
        foreach (var item in db)
        {
            if (item.ID == environment.ID)
            {
                db.Remove(item);
            }
        }
    }

    public Environment? Select(ulong id)
    {
        return db.Find(x => x.ID == id);
    }

    public List<Environment> Select()
    {
        return db;
    }
}