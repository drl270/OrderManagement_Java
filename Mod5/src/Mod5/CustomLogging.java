package Mod5;

import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;

public class CustomLogging 
{
    private static final Logger logger = Logger.getLogger(CustomLogging.class.getName());
    private static final LogFormatter formatter = new LogFormatter();
    private static LogHandler handler;

    static 
    {
        logger.setLevel(Level.ALL);
        try
        {
            handler = new LogHandler(formatter, "log.txt");
            logger.addHandler(handler);
        } 
        catch (IOException e) 
        {
            System.err.println("Error creating file: " + e.getMessage());
        }
    }

    public static void log(Level level, String message)
    {
        logger.log(level, message);
    }
}