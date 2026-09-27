Aplikacja internetowa typu **Expense Tracker** stworzona w technologii **ASP.NET Core MVC**. Pozwala użytkownikom na zarządzanie osobistym budżetem, śledzenie codziennych wydatków oraz analizowanie nawyków finansowych za pomocą interaktywnych wykresów.

---




# 🚀 Główne Funkcjonalności

## 1. 🔐 Autoryzacja i Rejestracja (ASP.NET Core Identity)
* **Hybrydowe logowanie:** Możliwość zalogowania się za pomocą **Nazwy użytkownika** lub **Adresu E-mail**.
* **Rejestracja:** Zakładanie kont z podaniem nazwy użytkownika oraz e-maila.
* **Bezpieczeństwo:** Dostęp do prywatnych funkcji (wydatki, analityka) jest chroniony przez atrybuty `[Authorize]`.
* **Wygoda:** Możliwość szybkiego utworzenia konta i logowania się za pomocą autoryzacji konta Google.

<details>
  <summary>🔍 Kliknij, aby zobaczyć zrzuty ekranu</summary>
  <img src="https://github.com/user-attachments/assets/406f4430-ab99-47f0-8f1b-4d3c8c73c19e" alt="Rejestracja" style="max-width: 100%; height: auto;" />
  <img src="https://github.com/user-attachments/assets/cc56c362-5260-4db4-bc54-b4c053e2afcb" alt="Logowanie" style="max-width: 100%; height: auto;" />
  
</details>

---

## 2. 📋 Zarządzanie Wydatkami (CRUD + LINQ)
* **Operacje CRUD:** Pełne tworzenie (Create), odczytywanie (Read), edycja (Update) oraz usuwanie (Delete) wpisów wydatków.
* **Wydajna filtracja i sortowanie (LINQ):** Przetwarzanie i wyszukiwanie danych po stronie serwera przy użyciu zapytań **LINQ**.
* **Zakresy dat i kategorie:** Kategoryzowanie wydatków oraz łatwy podgląd historii transakcji.
* **Dodawanie wielu wydatków na raz:** Możemy dodać plik CSV, który po poprawkach i zaakceptowaniu wydatków można w szybki sposób dodać do aplikacji.

<details>
  <summary>🔍 Kliknij, aby zobaczyć zrzuty ekranu</summary>
  Główna lista wydatków
  <img src="https://github.com/user-attachments/assets/72965636-f549-47c4-88f8-7292011b46cf" alt="Lista wydatków" style="max-width: 100%; height: auto;"/>
  
  Dodawanie wydatku - formularz
  <img src="https://github.com/user-attachments/assets/6bc1a7c9-a09f-4697-b6e6-6a0d953160f1" alt="Dodanie wydatku" style="max-width: 100%; height: auto;"/>

  Krótki formularz dodawania poprzez plik CSV
  <img src="https://github.com/user-attachments/assets/407820bc-4a59-45f1-985a-c1acb9659d8c" alt="Formularz CSV" style="max-width: 100%; height: auto;"/>
  Podgląd wydatków z możliwością edycji, np. poprawy kategorii
  <img src="https://github.com/user-attachments/assets/4aea024c-2651-4c34-8e58-451d2bdbfe31" alt="Podgląd wydatków" style="max-width: 100%; height: auto;"/>
  
  Funkcjonalność filtrów kategorii
  <img src="https://github.com/user-attachments/assets/0e242e53-b769-4108-8199-4d8e2b28bdc4" alt="filtr kategorii" style="max-width: 100%; height: auto;"/>
  
  Funkcjonalność filtru opisu
  <img src="https://github.com/user-attachments/assets/bb905eeb-10a2-4699-9558-281a94f731ea" alt="filtr opisu" style="max-width: 100%; height: auto;"/>
</details>



---

## 3. 📊 Analityka i Wykresy (Chart.js)
* **Dedykowany moduł:** Osobny `AnalyticsController` odpowiadający za agregację danych finansowych.
* **Wykresy dynamiczne:** 
  * Podsumowanie wydatków w podziale na **Kategorie** (wykres kołowy).
  * Śledzenie wydatków w ujęciu **Miesięcznym** (wykres słupkowy).
* **Agregacja w C#:** Zaawansowane zapytania `LINQ` (`GroupBy`, `Sum`) generujące gotowe tablice danych do podpięcia w JavaScript.

<details>
  <summary>🔍 Kliknij, aby zobaczyć zrzuty ekranu</summary>
  <img src="https://github.com/user-attachments/assets/5ea140cc-c5ba-4d32-b819-ba872c1b5630" alt="Wykresy" style="max-width: 100%; height: auto"/>

</details>

---

## 🛠️ Technologie i Biblioteki

* **Backend:** C# | .NET Core | ASP.NET Core MVC
* **Baza Danych & ORM:** Entity Framework Core | SQL Server (LocalDB)
* **Tożsamość:** ASP.NET Core Identity (`ApplicationUser`)
* **Frontend:** Razor Views (`.cshtml`) | Bootstrap | HTML5 / CSS3
* **Wykresy:** Chart.js / JavaScript

---

## 🗄️ Baza Danych i Konfiguracja
1. **Sklonuj repozytorium:**
   ```bash
   git clone [https://github.com/Axerrek/ExpenseManager.git](https://github.com/Axerrek/ExpenseManager.git)
