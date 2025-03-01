package Mod5;

import java.util.logging.Level;
import java.util.logging.Logger;

public class CustomLogging 
{
    private static final Logger logger = Logger.getLogger(CustomLogging.class.getName());
    private static final LogFormatter formatter = new LogFormatter();
    private static final LogHandler handler = new LogHandler(formatter);

    static
    {
        logger.setLevel(Level.ALL);
        logger.addHandler(handler);
    }

    public static void log(Level level, String message) 
    {
        logger.log(level, message);
    }
}