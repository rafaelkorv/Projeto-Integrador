import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:google_fonts/google_fonts.dart';
import 'screens/home_screen.dart';

void main() {
  SystemChrome.setSystemUIOverlayStyle(
    const SystemUiOverlayStyle(
      statusBarColor: Colors.transparent,
      statusBarIconBrightness: Brightness.dark,
    ),
  );
  runApp(const MyApp());
}

/// Tema Global do App Mobile — Fiel à Identidade Visual da Versão WEB do SocialJoin.
/// Fonte da Verdade de Estilo: web/style.css (Manrope, #EA3F74, #202124, #6b7280, #e5e7eb, radius 14px/10px, shadow-soft).
class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    final baseTextTheme = GoogleFonts.manropeTextTheme();

    const Color primaryPink = Color(0xFFEA3F74);
    const Color secondaryPink = Color(0xFFF9ACC6);
    const Color textMain = Color(0xFF202124);
    const Color textMuted = Color(0xFF6B7280);
    const Color borderColor = Color(0xFFE5E7EB);

    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'SocialJoin',
      theme: ThemeData(
        useMaterial3: true,
        primaryColor: primaryPink,
        scaffoldBackgroundColor: const Color(0xFFF5F7FF),
        textTheme: baseTextTheme.copyWith(
          displayLarge: baseTextTheme.displayLarge?.copyWith(color: textMain, fontWeight: FontWeight.w800),
          displayMedium: baseTextTheme.displayMedium?.copyWith(color: textMain, fontWeight: FontWeight.w800),
          displaySmall: baseTextTheme.displaySmall?.copyWith(color: textMain, fontWeight: FontWeight.w700),
          headlineLarge: baseTextTheme.headlineLarge?.copyWith(color: textMain, fontWeight: FontWeight.w800),
          headlineMedium: baseTextTheme.headlineMedium?.copyWith(color: textMain, fontWeight: FontWeight.w700),
          headlineSmall: baseTextTheme.headlineSmall?.copyWith(color: textMain, fontWeight: FontWeight.w700),
          titleLarge: baseTextTheme.titleLarge?.copyWith(color: textMain, fontWeight: FontWeight.w700),
          titleMedium: baseTextTheme.titleMedium?.copyWith(color: textMain, fontWeight: FontWeight.w700),
          titleSmall: baseTextTheme.titleSmall?.copyWith(color: textMain, fontWeight: FontWeight.w600),
          bodyLarge: baseTextTheme.bodyLarge?.copyWith(color: textMain),
          bodyMedium: baseTextTheme.bodyMedium?.copyWith(color: textMuted),
          bodySmall: baseTextTheme.bodySmall?.copyWith(color: textMuted),
          labelLarge: baseTextTheme.labelLarge?.copyWith(color: textMain, fontWeight: FontWeight.w700),
          labelMedium: baseTextTheme.labelMedium?.copyWith(color: textMuted, fontWeight: FontWeight.w600),
          labelSmall: baseTextTheme.labelSmall?.copyWith(color: textMuted, fontWeight: FontWeight.w800),
        ),
        colorScheme: ColorScheme.fromSeed(
          seedColor: primaryPink,
          primary: primaryPink,
          secondary: secondaryPink,
          surface: Colors.white,
          onPrimary: Colors.white,
          onSurface: textMain,
          tertiary: const Color(0xFF6C63FF),
        ),
        appBarTheme: AppBarTheme(
          backgroundColor: Colors.white,
          foregroundColor: textMain,
          elevation: 0,
          surfaceTintColor: Colors.transparent,
          centerTitle: false,
          titleTextStyle: GoogleFonts.manrope(
            color: textMain,
            fontSize: 18,
            fontWeight: FontWeight.w700,
          ),
          iconTheme: const IconThemeData(
            color: textMain,
            size: 22,
          ),
        ),
        cardTheme: CardThemeData(
          elevation: 0,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(18),
            side: const BorderSide(color: borderColor, width: 1),
          ),
          color: Colors.white,
          surfaceTintColor: Colors.transparent,
          margin: EdgeInsets.zero,
        ),
        inputDecorationTheme: InputDecorationTheme(
          filled: true,
          fillColor: Colors.white,
          contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: const BorderSide(color: borderColor, width: 1),
          ),
          enabledBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: const BorderSide(color: borderColor, width: 1),
          ),
          focusedBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: const BorderSide(color: primaryPink, width: 1.5),
          ),
          errorBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: const BorderSide(color: Color(0xFFC93659), width: 1),
          ),
          focusedErrorBorder: OutlineInputBorder(
            borderRadius: BorderRadius.circular(12),
            borderSide: const BorderSide(color: Color(0xFFC93659), width: 1.5),
          ),
          hintStyle: GoogleFonts.manrope(color: textMuted, fontSize: 14),
          labelStyle: GoogleFonts.manrope(color: textMuted, fontSize: 14),
        ),
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ElevatedButton.styleFrom(
            backgroundColor: primaryPink,
            foregroundColor: Colors.white,
            elevation: 0,
            padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 12),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12),
            ),
            textStyle: GoogleFonts.manrope(
              fontSize: 14,
              fontWeight: FontWeight.w600,
            ),
          ),
        ),
        outlinedButtonTheme: OutlinedButtonThemeData(
          style: OutlinedButton.styleFrom(
            foregroundColor: primaryPink,
            side: const BorderSide(color: secondaryPink, width: 1),
            padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 12),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12),
            ),
            textStyle: GoogleFonts.manrope(
              fontSize: 14,
              fontWeight: FontWeight.w600,
            ),
          ),
        ),
        textButtonTheme: TextButtonThemeData(
          style: TextButton.styleFrom(
            foregroundColor: primaryPink,
            textStyle: GoogleFonts.manrope(
              fontSize: 13,
              fontWeight: FontWeight.w700,
            ),
          ),
        ),
        chipTheme: ChipThemeData(
          backgroundColor: Colors.white,
          selectedColor: primaryPink,
          labelStyle: GoogleFonts.manrope(fontSize: 12, fontWeight: FontWeight.w700),
          side: const BorderSide(color: borderColor),
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(999)),
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
        ),
        dividerTheme: const DividerThemeData(
          color: borderColor,
          thickness: 1,
          space: 0,
        ),
        bottomSheetTheme: const BottomSheetThemeData(
          backgroundColor: Colors.white,
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
          ),
          surfaceTintColor: Colors.transparent,
        ),
        dialogTheme: DialogThemeData(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
          surfaceTintColor: Colors.transparent,
        ),
        snackBarTheme: SnackBarThemeData(
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        ),
        floatingActionButtonTheme: const FloatingActionButtonThemeData(
          backgroundColor: primaryPink,
          foregroundColor: Colors.white,
        ),
      ),
      home: const HomeScreen(),
    );
  }
}
