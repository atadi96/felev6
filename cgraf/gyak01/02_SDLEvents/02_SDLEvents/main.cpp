#include <SDL.h>
#include <iostream>
#define _USE_MATH_DEFINES
#include "math.h"

void exitProgram()
{
	std::cout << "Kilépéshez nyomj meg egy billentyût..." << std::endl;
	std::cin.get();
}

void red(int& r, int& g, int& b) {
	r = 255;
	g = b = 0;
}

void blue(int& r, int& g, int& b) {
	b = 255;
	g = r = 0;
}

int main( int argc, char* args[] )
{
	atexit( exitProgram );
	//
	// 1. lépés: inicializáljuk az SDL-t
	//

	// a grafikus alrendszert kapcsoljuk csak be, ha gond van, akkor jelezzük és lépjün ki
	if ( SDL_Init( SDL_INIT_VIDEO ) == -1 )
	{
		// irjuk ki a hibat es terminaljon a program
		std::cout << "[SDL indítása]Hiba az SDL inicializálása közben: " << SDL_GetError() << std::endl;
		return 1;
	}
			
	//
	// 2. lépés: hozzuk létre az ablakot, amire rajzolni fogunk
	//

	SDL_Window *win = nullptr;
    win = SDL_CreateWindow( "Hello SDL!",				// az ablak fejléce
							100,						// az ablak bal-felsõ sarkának kezdeti X koordinátája
							100,						// az ablak bal-felsõ sarkának kezdeti Y koordinátája
							640,						// ablak szélessége
							480,						// és magassága
							SDL_WINDOW_SHOWN);			// megjelenítési tulajdonságok

	// ha nem sikerült létrehozni az ablakot, akkor írjuk ki a hibát, amit kaptunk és lépjünk ki
    if (win == nullptr)
	{
		std::cout << "[Ablak létrehozása]Hiba az SDL inicializálása közben: " << SDL_GetError() << std::endl;
        return 1;
    }

	//
	// 3. lépés: hozzunk létre egy renderelõt, rajzolót
	//

    SDL_Renderer *ren = nullptr;
    ren = SDL_CreateRenderer(	win, // melyik ablakhoz rendeljük hozzá a renderert
								-1,  // melyik indexú renderert inicializáljuka
									 // a -1 a harmadik paraméterben meghatározott igényeinknek megfelelõ elsõ renderelõt jelenti
								SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);	// az igényeink, azaz
																						// hardveresen gyorsított és vsync-et beváró
    if (ren == nullptr)
	{
        std::cout << "[Renderer létrehozása]Hiba az SDL inicializálása közben: " << SDL_GetError() << std::endl;
        return 1;
    }

	//
	// 4. lépés: indítsuk el a fõ üzenetfeldolgozó ciklust
	// 

	// véget kell-e érjen a program futása?
	bool quit = false;
	// feldolgozandó üzenet ide kerül
	SDL_Event ev;
	// egér X és Y koordinátái
	Sint32 mouseX = 0, mouseY = 0;
	int r, g, b;
	r = 255;
	g = b = 0;
	int frame = 0;
	bool newMode = false;

	while (!quit)
	{
		++frame;
		// amíg van feldolgozandó üzenet dolgozzuk fel mindet:
		while ( SDL_PollEvent(&ev) )
		{
			switch (ev.type)
			{
			case SDL_QUIT:
				quit = true;
				break;
			case SDL_KEYDOWN:
				if ( ev.key.keysym.sym == SDLK_ESCAPE )
					quit = true;
				if (ev.key.keysym.sym == SDLK_a) {
					newMode = !newMode;
				}
				break;
			case SDL_MOUSEMOTION:
				mouseX = ev.motion.x;
				mouseY = ev.motion.y;
				break;
			case SDL_MOUSEBUTTONUP:
				// egérgomb felengedésének eseménye; a felengedett gomb a ev.button.button -ban található
				// a lehetséges gombok a következõek: SDL_BUTTON_LEFT, SDL_BUTTON_MIDDLE, 
				//		SDL_BUTTON_RIGHT, SDL_BUTTON_WHEELUP, SDL_BUTTON_WHEELDOWN
				if (ev.button.button == SDL_BUTTON_LEFT)
				{
					red(r, g, b);
				}
				if (ev.button.button == SDL_BUTTON_RIGHT)
				{
					blue(r, g, b);
				}
				break;
			}
		}

		// töröljük a hátteret fehérre
		SDL_SetRenderDrawColor(ren, 255, 255, 255, 255);
		SDL_RenderClear(ren);

		// aktuális rajzolási szín legyen zöld és rajzoljunk ki egy vonalat
		SDL_SetRenderDrawColor(	ren,	// renderer címe, aminek a rajzolási színét be akarjuk állítani
								0,		// piros
								255,	// zöld
								0,		// kék
								255);	// átlátszatlanság

		SDL_RenderDrawLine(	ren,	// renderer címe, ahová vonalat akarunk rajzolni
							0, 0, // vonal kezdõpontjának (x,y) koordinátái
							mouseX, mouseY);// vonal végpontjának (x,y) koordinátái
		
		// definiáljunk egy (mouseX, mouseY) középpontó, tengelyekkel párhuzamos oldalú
		// 20x20-as négyzetet:
		SDL_Rect cursor_rect;
		if (newMode)
		{
			int mouseKEK = mouseX % 110 + 1;
			if (90 <= mouseKEK && mouseKEK <= 110)
			{
				blue(r, g, b);
			}
			else
			{
				red(r, g, b);
			}
			cursor_rect.x = mouseX - 10;
			cursor_rect.y = mouseY - 10;
			cursor_rect.w = 20;
			cursor_rect.h = 20;
		}
		else
		{
			//int size = abs(sin(sin(frame) / (5 * M_PI))) * 100;
			int size = abs(sin(frame / (5 * M_PI))) * 100 + sin(frame / 7.0) * 50;
			cursor_rect.x = mouseX - 10 - size / 2 - sin(frame / 10.) * 40;
			cursor_rect.y = mouseY - 10 - size / 2 - cos(frame / 10.) * 40;
			cursor_rect.w = 20 + size;
			cursor_rect.h = 20 + size;
		}

		// legyen a kitöltési szín piros
		SDL_SetRenderDrawColor( ren, r, g, b, 255 );
		SDL_RenderFillRect( ren, &cursor_rect);

		// 1. feladat: az eltelt idõ függvényében periodikusan nõjjön és csökkenjen
		//    az egérmutató középpontjával kirajzolt négyszög

		// 2. feladat: ha a user a bal egérgombot nyomja meg akkor a téglalap színe váltson pirosra,
		//    ha a jobb egérgombot, akkor kékre

		// 3. feladat: rajzolj ki egy 50 sugarú körvonalat az egérmutató köré!
		// segítség: használd a SDL_RenderDrawLines()-t

		// jelenítsük meg a backbuffer tartalmát
		SDL_RenderPresent(ren);
	}



	//
	// 4. lépés: lépjünk ki
	// 

	SDL_DestroyRenderer( ren );
	SDL_DestroyWindow( win );

	SDL_Quit();

	return 0;
}