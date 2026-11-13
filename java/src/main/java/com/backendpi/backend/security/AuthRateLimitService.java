package com.backendpi.backend.security;

import java.time.Duration;
import java.util.LinkedHashMap;
import java.util.Map;

import org.springframework.stereotype.Service;

@Service
public class AuthRateLimitService {

    private static final int MAX_TRACKED_CLIENTS = 10_000;

    private final Map<String, AttemptWindow> windows =
            new LinkedHashMap<>(16, 0.75f, true);

    public synchronized long tryAcquire(
            String clientKey,
            int maxAttempts,
            Duration windowDuration) {
        long now = System.currentTimeMillis();
        long durationMillis = windowDuration.toMillis();
        AttemptWindow window = windows.get(clientKey);

        if (window == null || now - window.startedAtMillis >= durationMillis) {
            if (window == null && windows.size() >= MAX_TRACKED_CLIENTS) {
                String oldestKey = windows.keySet().iterator().next();
                windows.remove(oldestKey);
            }
            windows.put(clientKey, new AttemptWindow(now, 1));
            return 0;
        }

        if (window.attempts >= maxAttempts) {
            long remainingMillis = durationMillis - (now - window.startedAtMillis);
            return Math.max(1, (remainingMillis + 999) / 1000);
        }

        window.attempts++;
        return 0;
    }

    private static final class AttemptWindow {
        private final long startedAtMillis;
        private int attempts;

        private AttemptWindow(long startedAtMillis, int attempts) {
            this.startedAtMillis = startedAtMillis;
            this.attempts = attempts;
        }
    }
}
