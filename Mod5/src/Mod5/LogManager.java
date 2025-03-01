package Mod5;

import java.util.logging.Level;
import java.util.logging.Logger;

public class LogManager 
{
    private static final Logger logger = Logger.getLogger(LogManager.class.getName());

    public void log(Level level, String message)
    {
        logger.log(level, message);
    }

    public void setLevel(Level level) 
    {
        logger.setLevel(level);
    }
}