#include <QmlNet/qml/QQmlApplicationEngine.h>
#include <QmlNet/types/NetTypeInfo.h>
#include <QmlNet/qml/NetValueType.h>
#include <QmlNet/types/Callbacks.h>
#include <QmlNet/qml/JsNetObject.h>
#include <QQmlContext>

static int netValueTypeNumber = 0;

#define NETVALUETYPE_CASE(N) \
    case N: NetValueType<N>::init(typeInfo); return qmlRegisterType<NetValueType<(N)>>(uriString.toUtf8().data(), versionMajor, versionMinor, qmlNameString.toUtf8().data());

#define NETVALUETYPESINGLETON_CASE(N) \
    case N: NetValueType<N>::init(typeInfo); return qmlRegisterSingletonType<NetValueType<N>>(uriString.toUtf8().data(), versionMajor, versionMinor, typeNameString.toUtf8().data(), NetValueType<N>::build);


extern "C" {

Q_DECL_EXPORT QQmlApplicationEngineContainer* qqmlapplicationengine_create(QQmlApplicationEngine* existingEngine) {
    bool ownsEngine = true;
    QQmlApplicationEngine* engine = nullptr;

    if (existingEngine != nullptr) {
        engine = existingEngine;
        ownsEngine = false;
    } else {
        engine = new QQmlApplicationEngine();
        ownsEngine = true;
    }

    JsNetObject* netObject = new JsNetObject();

    engine->rootContext()->setContextProperty("Net", netObject);

    return new QQmlApplicationEngineContainer{
        engine,
        netObject,
        ownsEngine
    };
}

Q_DECL_EXPORT void qqmlapplicationengine_destroy(QQmlApplicationEngineContainer* container) {
    if(container->ownsEngine) {
        delete container->qmlEngine;
    }
    delete container->netObject;
    delete container;
}

Q_DECL_EXPORT void qqmlapplicationengine_load(QQmlApplicationEngineContainer* container, LPWSTR path) {
    container->qmlEngine->load(QString::fromUtf16(static_cast<const char16_t*>(path)));
}

Q_DECL_EXPORT void qqmlapplicationengine_loadData(QQmlApplicationEngineContainer* container, LPWSTR dataString) {
    container->qmlEngine->loadData(QByteArray::fromStdString(QString::fromUtf16(static_cast<const char16_t*>(dataString)).toStdString()));
}

Q_DECL_EXPORT int qqmlapplicationengine_registerType(NetTypeInfoContainer* typeContainer, LPWSTR uri, int versionMajor, int versionMinor, LPWSTR qmlName) {

    QString uriString = QString::fromUtf16(static_cast<const char16_t*>(uri));
    QString qmlNameString = QString::fromUtf16(static_cast<const char16_t*>(qmlName));
    QSharedPointer<NetTypeInfo> typeInfo = typeContainer->netTypeInfo;

    switch (++netValueTypeNumber) {
        NETVALUETYPE_CASE(1)
        NETVALUETYPE_CASE(2)
        NETVALUETYPE_CASE(3)
        NETVALUETYPE_CASE(4)
        NETVALUETYPE_CASE(5)
        NETVALUETYPE_CASE(6)
        NETVALUETYPE_CASE(7)
        NETVALUETYPE_CASE(8)
        NETVALUETYPE_CASE(9)
        NETVALUETYPE_CASE(10)
        NETVALUETYPE_CASE(11)
        NETVALUETYPE_CASE(12)
        NETVALUETYPE_CASE(13)
        NETVALUETYPE_CASE(14)
        NETVALUETYPE_CASE(15)
        NETVALUETYPE_CASE(16)
        NETVALUETYPE_CASE(17)
        NETVALUETYPE_CASE(18)
        NETVALUETYPE_CASE(19)
        NETVALUETYPE_CASE(20)
        NETVALUETYPE_CASE(21)
        NETVALUETYPE_CASE(22)
        NETVALUETYPE_CASE(23)
        NETVALUETYPE_CASE(24)
        NETVALUETYPE_CASE(25)
        NETVALUETYPE_CASE(26)
        NETVALUETYPE_CASE(27)
        NETVALUETYPE_CASE(28)
        NETVALUETYPE_CASE(29)
        NETVALUETYPE_CASE(30)
        NETVALUETYPE_CASE(31)
        NETVALUETYPE_CASE(32)
        NETVALUETYPE_CASE(33)
        NETVALUETYPE_CASE(34)
        NETVALUETYPE_CASE(35)
        NETVALUETYPE_CASE(36)
        NETVALUETYPE_CASE(37)
        NETVALUETYPE_CASE(38)
        NETVALUETYPE_CASE(39)
        NETVALUETYPE_CASE(40)
        NETVALUETYPE_CASE(41)
        NETVALUETYPE_CASE(42)
        NETVALUETYPE_CASE(43)
        NETVALUETYPE_CASE(44)
        NETVALUETYPE_CASE(45)
        NETVALUETYPE_CASE(46)
        NETVALUETYPE_CASE(47)
        NETVALUETYPE_CASE(48)
        NETVALUETYPE_CASE(49)
        NETVALUETYPE_CASE(50)
        NETVALUETYPE_CASE(51)
        NETVALUETYPE_CASE(52)
        NETVALUETYPE_CASE(53)
        NETVALUETYPE_CASE(54)
        NETVALUETYPE_CASE(55)
        NETVALUETYPE_CASE(56)
        NETVALUETYPE_CASE(57)
        NETVALUETYPE_CASE(58)
        NETVALUETYPE_CASE(59)
        NETVALUETYPE_CASE(60)
        NETVALUETYPE_CASE(61)
        NETVALUETYPE_CASE(62)
        NETVALUETYPE_CASE(63)
        NETVALUETYPE_CASE(64)
        NETVALUETYPE_CASE(65)
        NETVALUETYPE_CASE(66)
        NETVALUETYPE_CASE(67)
        NETVALUETYPE_CASE(68)
        NETVALUETYPE_CASE(69)
        NETVALUETYPE_CASE(70)
        NETVALUETYPE_CASE(71)
        NETVALUETYPE_CASE(72)
        NETVALUETYPE_CASE(73)
        NETVALUETYPE_CASE(74)
        NETVALUETYPE_CASE(75)
        NETVALUETYPE_CASE(76)
        NETVALUETYPE_CASE(77)
        NETVALUETYPE_CASE(78)
        NETVALUETYPE_CASE(79)
        NETVALUETYPE_CASE(80)
        NETVALUETYPE_CASE(81)
        NETVALUETYPE_CASE(82)
        NETVALUETYPE_CASE(83)
        NETVALUETYPE_CASE(84)
        NETVALUETYPE_CASE(85)
        NETVALUETYPE_CASE(86)
        NETVALUETYPE_CASE(87)
        NETVALUETYPE_CASE(88)
        NETVALUETYPE_CASE(89)
        NETVALUETYPE_CASE(90)
        NETVALUETYPE_CASE(91)
        NETVALUETYPE_CASE(92)
        NETVALUETYPE_CASE(93)
        NETVALUETYPE_CASE(94)
        NETVALUETYPE_CASE(95)
        NETVALUETYPE_CASE(96)
        NETVALUETYPE_CASE(97)
        NETVALUETYPE_CASE(98)
        NETVALUETYPE_CASE(99)
        NETVALUETYPE_CASE(100)
        NETVALUETYPE_CASE(101)
        NETVALUETYPE_CASE(102)
        NETVALUETYPE_CASE(103)
        NETVALUETYPE_CASE(104)
        NETVALUETYPE_CASE(105)
        NETVALUETYPE_CASE(106)
        NETVALUETYPE_CASE(107)
        NETVALUETYPE_CASE(108)
        NETVALUETYPE_CASE(109)
        NETVALUETYPE_CASE(110)
        NETVALUETYPE_CASE(111)
        NETVALUETYPE_CASE(112)
        NETVALUETYPE_CASE(113)
        NETVALUETYPE_CASE(114)
        NETVALUETYPE_CASE(115)
        NETVALUETYPE_CASE(116)
        NETVALUETYPE_CASE(118)
        NETVALUETYPE_CASE(119)
        NETVALUETYPE_CASE(120)
        NETVALUETYPE_CASE(121)
        NETVALUETYPE_CASE(122)
        NETVALUETYPE_CASE(123)
        NETVALUETYPE_CASE(124)
        NETVALUETYPE_CASE(125)
    }
    qFatal("Too many registered types");
    return -1;
}

Q_DECL_EXPORT int qqmlapplicationengine_registerSingletonTypeQml(LPWCSTR url, LPWCSTR uri, int versionMajor, int versionMinor, LPWCSTR qmlName)
{
    QString urlString = QString::fromUtf16(url);
    QString uriString = QString::fromUtf16(uri);
    QString qmlNameString = QString::fromUtf16(qmlName);
    return qmlRegisterSingletonType(urlString, uriString.toUtf8().data(), versionMajor, versionMinor, qmlNameString.toUtf8().data());
}

Q_DECL_EXPORT int qqmlapplicationengine_registerSingletonTypeNet(NetTypeInfoContainer* typeContainer, LPWCSTR uri, int versionMajor, int versionMinor, LPWCSTR typeName)
{
    QSharedPointer<NetTypeInfo> typeInfo = typeContainer->netTypeInfo;
    QString typeNameString = QString::fromUtf16(typeName);
    QString uriString = QString::fromUtf16(uri);

    switch (++netValueTypeNumber) {
        NETVALUETYPESINGLETON_CASE(1)
        NETVALUETYPESINGLETON_CASE(2)
        NETVALUETYPESINGLETON_CASE(3)
        NETVALUETYPESINGLETON_CASE(4)
        NETVALUETYPESINGLETON_CASE(5)
        NETVALUETYPESINGLETON_CASE(6)
        NETVALUETYPESINGLETON_CASE(7)
        NETVALUETYPESINGLETON_CASE(8)
        NETVALUETYPESINGLETON_CASE(9)
        NETVALUETYPESINGLETON_CASE(10)
        NETVALUETYPESINGLETON_CASE(11)
        NETVALUETYPESINGLETON_CASE(12)
        NETVALUETYPESINGLETON_CASE(13)
        NETVALUETYPESINGLETON_CASE(14)
        NETVALUETYPESINGLETON_CASE(15)
        NETVALUETYPESINGLETON_CASE(16)
        NETVALUETYPESINGLETON_CASE(17)
        NETVALUETYPESINGLETON_CASE(18)
        NETVALUETYPESINGLETON_CASE(19)
        NETVALUETYPESINGLETON_CASE(20)
        NETVALUETYPESINGLETON_CASE(21)
        NETVALUETYPESINGLETON_CASE(22)
        NETVALUETYPESINGLETON_CASE(23)
        NETVALUETYPESINGLETON_CASE(24)
        NETVALUETYPESINGLETON_CASE(25)
        NETVALUETYPESINGLETON_CASE(26)
        NETVALUETYPESINGLETON_CASE(27)
        NETVALUETYPESINGLETON_CASE(28)
        NETVALUETYPESINGLETON_CASE(29)
        NETVALUETYPESINGLETON_CASE(30)
        NETVALUETYPESINGLETON_CASE(31)
        NETVALUETYPESINGLETON_CASE(32)
        NETVALUETYPESINGLETON_CASE(33)
        NETVALUETYPESINGLETON_CASE(34)
        NETVALUETYPESINGLETON_CASE(35)
        NETVALUETYPESINGLETON_CASE(36)
        NETVALUETYPESINGLETON_CASE(37)
        NETVALUETYPESINGLETON_CASE(38)
        NETVALUETYPESINGLETON_CASE(39)
        NETVALUETYPESINGLETON_CASE(40)
        NETVALUETYPESINGLETON_CASE(41)
        NETVALUETYPESINGLETON_CASE(42)
        NETVALUETYPESINGLETON_CASE(43)
        NETVALUETYPESINGLETON_CASE(44)
        NETVALUETYPESINGLETON_CASE(45)
        NETVALUETYPESINGLETON_CASE(46)
        NETVALUETYPESINGLETON_CASE(47)
        NETVALUETYPESINGLETON_CASE(48)
        NETVALUETYPESINGLETON_CASE(49)
        NETVALUETYPESINGLETON_CASE(50)
        NETVALUETYPESINGLETON_CASE(51)
        NETVALUETYPESINGLETON_CASE(52)
        NETVALUETYPESINGLETON_CASE(53)
        NETVALUETYPESINGLETON_CASE(54)
        NETVALUETYPESINGLETON_CASE(55)
        NETVALUETYPESINGLETON_CASE(56)
        NETVALUETYPESINGLETON_CASE(57)
        NETVALUETYPESINGLETON_CASE(58)
        NETVALUETYPESINGLETON_CASE(59)
        NETVALUETYPESINGLETON_CASE(60)
        NETVALUETYPESINGLETON_CASE(61)
        NETVALUETYPESINGLETON_CASE(62)
        NETVALUETYPESINGLETON_CASE(63)
        NETVALUETYPESINGLETON_CASE(64)
        NETVALUETYPESINGLETON_CASE(65)
        NETVALUETYPESINGLETON_CASE(66)
        NETVALUETYPESINGLETON_CASE(67)
        NETVALUETYPESINGLETON_CASE(68)
        NETVALUETYPESINGLETON_CASE(69)
        NETVALUETYPESINGLETON_CASE(70)
        NETVALUETYPESINGLETON_CASE(71)
        NETVALUETYPESINGLETON_CASE(72)
        NETVALUETYPESINGLETON_CASE(73)
        NETVALUETYPESINGLETON_CASE(74)
        NETVALUETYPESINGLETON_CASE(75)
        NETVALUETYPESINGLETON_CASE(76)
        NETVALUETYPESINGLETON_CASE(77)
        NETVALUETYPESINGLETON_CASE(78)
        NETVALUETYPESINGLETON_CASE(79)
        NETVALUETYPESINGLETON_CASE(80)
        NETVALUETYPESINGLETON_CASE(81)
        NETVALUETYPESINGLETON_CASE(82)
        NETVALUETYPESINGLETON_CASE(83)
        NETVALUETYPESINGLETON_CASE(84)
        NETVALUETYPESINGLETON_CASE(85)
        NETVALUETYPESINGLETON_CASE(86)
        NETVALUETYPESINGLETON_CASE(87)
        NETVALUETYPESINGLETON_CASE(88)
        NETVALUETYPESINGLETON_CASE(89)
        NETVALUETYPESINGLETON_CASE(90)
        NETVALUETYPESINGLETON_CASE(91)
        NETVALUETYPESINGLETON_CASE(92)
        NETVALUETYPESINGLETON_CASE(93)
        NETVALUETYPESINGLETON_CASE(94)
        NETVALUETYPESINGLETON_CASE(95)
        NETVALUETYPESINGLETON_CASE(96)
        NETVALUETYPESINGLETON_CASE(97)
        NETVALUETYPESINGLETON_CASE(98)
        NETVALUETYPESINGLETON_CASE(99)
        NETVALUETYPESINGLETON_CASE(100)
        NETVALUETYPESINGLETON_CASE(101)
        NETVALUETYPESINGLETON_CASE(102)
        NETVALUETYPESINGLETON_CASE(103)
        NETVALUETYPESINGLETON_CASE(104)
        NETVALUETYPESINGLETON_CASE(105)
        NETVALUETYPESINGLETON_CASE(106)
        NETVALUETYPESINGLETON_CASE(107)
        NETVALUETYPESINGLETON_CASE(108)
        NETVALUETYPESINGLETON_CASE(109)
        NETVALUETYPESINGLETON_CASE(110)
        NETVALUETYPESINGLETON_CASE(111)
        NETVALUETYPESINGLETON_CASE(112)
        NETVALUETYPESINGLETON_CASE(113)
        NETVALUETYPESINGLETON_CASE(114)
        NETVALUETYPESINGLETON_CASE(115)
        NETVALUETYPESINGLETON_CASE(116)
        NETVALUETYPESINGLETON_CASE(118)
        NETVALUETYPESINGLETON_CASE(119)
        NETVALUETYPESINGLETON_CASE(120)
        NETVALUETYPESINGLETON_CASE(121)
        NETVALUETYPESINGLETON_CASE(122)
        NETVALUETYPESINGLETON_CASE(123)
        NETVALUETYPESINGLETON_CASE(124)
        NETVALUETYPESINGLETON_CASE(125)
    }
    qFatal("Too many registered types");
    return -1;
}

Q_DECL_EXPORT void qqmlapplicationengine_addImportPath(QQmlApplicationEngineContainer* container, LPWSTR path) {
    QString pathString = QString::fromUtf16(static_cast<const char16_t*>(path));
    container->qmlEngine->addImportPath(pathString);
}

Q_DECL_EXPORT QQmlApplicationEngine* qqmlapplicationengine_internalPointer(QQmlApplicationEngineContainer* container) {
    return container->qmlEngine;
}

Q_DECL_EXPORT NetVariantContainer* qqmlapplicationengine_getContextProperty(QQmlApplicationEngineContainer* container, LPWCSTR name)
{
    QVariant result = container->qmlEngine->rootContext()->contextProperty(QString::fromUtf16(name));
    return new NetVariantContainer {
        NetVariant::fromQVariant(&result)
    };
}

Q_DECL_EXPORT void qqmlapplicationengine_setContextProperty(QQmlApplicationEngineContainer* container, LPWCSTR name, NetVariantContainer* valueContainer)
{
    if(valueContainer == nullptr) {
        container->qmlEngine->rootContext()->setContextProperty(QString::fromUtf16(name), nullptr);
    } else {
        QSharedPointer<NetVariant> value = valueContainer->variant;
        container->qmlEngine->rootContext()->setContextProperty(QString::fromUtf16(name), value->toQVariant());
    }
}

}
